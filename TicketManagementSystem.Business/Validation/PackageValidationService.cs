using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Validation
{
    public class PackageValidationService : ValidationService, IPackageValidationService
    {
        private readonly IPackageService _packageService;
        private readonly IColorService _colorService;
        private readonly ISerialService _serialService;

        public PackageValidationService(IUnitOfWork database, IPackageService packageService, IColorService colorService, ISerialService serialService) : base(database)
        {
            _packageService = packageService;
            _colorService = colorService;
            _serialService = serialService;
        }

        public IEnumerable<string> Validate(PackageCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (!_colorService.ExistsById(createDTO.ColorId))
            {
                errors.Add($"Кольору ID: {createDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(createDTO.SerialId))
            {
                errors.Add($"Серії ID: {createDTO.SerialId} не існує.");
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageSpecialCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (_packageService.ExistsByName(createDTO.Name))
            {
                errors.Add($"Пачка \"{createDTO.Name}\" вже існує.");
            }

            if (createDTO.ColorId != null && !_colorService.ExistsById((int)createDTO.ColorId))
            {
                errors.Add($"Кольору ID: {createDTO.ColorId} не існує.");
            }

            if (createDTO.SerialId != null && !_serialService.ExistsById((int)createDTO.SerialId))
            {
                errors.Add($"Серії ID: {createDTO.SerialId} не існує.");
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));

            if (!_colorService.ExistsById(editDTO.ColorId))
            {
                errors.Add($"Кольору ID: {editDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(editDTO.SerialId))
            {
                errors.Add($"Серії ID: {editDTO.SerialId} не існує.");
            }

            var packageTickets = _packageService.GetPackageTickets(editDTO.Id)?.ToList();

            if (packageTickets?.Any() == true && editDTO.FirstNumber != null)
            {
                if (!packageTickets.TrueForAll(t => int.Parse(t.Number.First().ToString()) == editDTO.FirstNumber))
                {
                    errors.Add("Для цієї пачки неможливо встановити першу цифру.");
                }
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageSpecialEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));

            if (!_packageService.IsNameFree(editDTO.Id, editDTO.Name))
            {
                errors.Add($"Пачка \"{editDTO.Name}\" вже існує.");
            }

            if (editDTO.ColorId != null && !_colorService.ExistsById((int)editDTO.ColorId))
            {
                errors.Add($"Кольору ID: {editDTO.ColorId} не існує.");
            }

            if (editDTO.SerialId != null && !_serialService.ExistsById((int)editDTO.SerialId))
            {
                errors.Add($"Серії ID: {editDTO.SerialId} не існує.");
            }

            var packageTickets = _packageService.GetPackageTickets(editDTO.Id)?.ToList();

            if (packageTickets?.Any() == true)
            {
                if (!packageTickets.TrueForAll(t => t.ColorId == editDTO.ColorId) && editDTO.ColorId != null)
                {
                    errors.Add("Для цієї пачки неможливо встановити єдиний колір.");
                }

                if (!packageTickets.TrueForAll(t => t.SerialId == editDTO.SerialId) && editDTO.SerialId != null)
                {
                    errors.Add("Для цієї пачки неможливо встановити єдину серію.");
                }
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageMakeDefaultDTO dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(dto));

            var package = Database.Packages.GetById(dto.Id);

            if (!package.IsSpecial)
            {
                errors.Add("Пачка й так звичайна");
                return errors;
            }

            var tickets = _packageService.GetPackageTickets(dto.Id)?.ToList();

            if (!_colorService.ExistsById(dto.ColorId))
            {
                errors.Add($"Кольору ID: {dto.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(dto.SerialId))
            {
                errors.Add($"Серії ID: {dto.SerialId} не існує.");
            }

            if (tickets?.Any() == true)
            {
                if (dto.FirstNumber != null && !tickets.TrueForAll(t => int.Parse(t.Number.First().ToString()) == dto.FirstNumber))
                {
                    errors.Add("Для цієї пачки неможливо встановити першу цифру.");
                }

                if (!tickets.TrueForAll(t => t.ColorId == dto.ColorId))
                {
                    errors.Add("Для цієї пачки неможливо встановити серію.");
                }

                if (!tickets.TrueForAll(t => t.SerialId == dto.SerialId))
                {
                    errors.Add("Для цієї пачки неможливо встановити колір.");
                }
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageMakeSpecialDTO dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(dto));

            var package = Database.Packages.GetById(dto.Id);

            if (package.IsSpecial)
            {
                errors.Add("Пачка й так спеціальна");
                return errors;
            }

            if (_packageService.ExistsByName(dto.Name))
                errors.Add($"Пачка \"{dto.Name}\" вже існує.");

            return errors;
        }
    }
}
