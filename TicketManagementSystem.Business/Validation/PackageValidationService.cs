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
