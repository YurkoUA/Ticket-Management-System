using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Validation
{
    public class TicketValidationService : ValidationService, ITicketValidationService
    {
        private readonly ITicketService _ticketService;
        private readonly IPackageService _packageService;
        private readonly ISerialService _serialService;
        private readonly IColorService _colorService;

        public TicketValidationService(
            IUnitOfWork database,
            ITicketService ticketService,
            IPackageService packageService,
            ISerialService serialService,
            IColorService colorService) : base(database)
        {
            _ticketService = ticketService;
            _packageService = packageService;
            _serialService = serialService;
            _colorService = colorService;
        }

        public IEnumerable<string> Validate(TicketCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (_ticketService.Exists(Mapper.Map<TicketDTO>(createDTO)))
            {
                errors.Add("Такий квиток вже існує.");
            }

            if (!_colorService.ExistsById(createDTO.ColorId))
            {
                errors.Add($"Кольору ID: {createDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(createDTO.SerialId))
            {
                errors.Add($"Серії ID: {createDTO.SerialId} не існує.");
            }

            if (createDTO.PackageId != null)
            {
                var package = Database.Packages.GetById((int)createDTO.PackageId);

                if (package == null)
                {
                    errors.Add($"Пачки ID: {createDTO.PackageId} не існує.");
                }
                else if (!package.IsOpened)
                {
                    errors.Add($"Пачка \"{package.ToString()}\" закрита.");
                }
                else if (package.FirstNumber != null && package.FirstNumber != int.Parse(createDTO.Number.First().ToString()))
                {
                    errors.Add($"До пачки \"{package.ToString()}\" можна додавати лише квитки на цифру {package.FirstNumber}.");
                }
                else
                {
                    if (package.ColorId != null && package.ColorId != createDTO.ColorId)
                    {
                        errors.Add("Колір квитка не збігається з кольором пачки.");
                    }

                    if (package.SerialId != null && package.SerialId != createDTO.SerialId)
                    {
                        errors.Add("Серія квитка не збігається з серією пачки.");
                    }
                }
            }
            return errors;
        }

        public IEnumerable<string> Validate(TicketEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));

            var ticket = Database.Tickets.GetById(editDTO.Id);

            var dtoForCheckExisting = Mapper.Map<TicketDTO>(editDTO);
            dtoForCheckExisting.Number = ticket.Number;

            if (_ticketService.Exists(dtoForCheckExisting, editDTO.Id))
            {
                errors.Add("Такий квиток вже існує.");
            }

            if (!_colorService.ExistsById(editDTO.ColorId))
            {
                errors.Add($"Кольору ID: {editDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(editDTO.SerialId))
            {
                errors.Add($"Серії ID: {editDTO.SerialId} не існує.");
            }

            if (ticket.PackageId != null)
            {
                var package = Database.Packages.GetById((int)ticket.PackageId);

                if (package.ColorId != null && package.ColorId != editDTO.ColorId)
                {
                    errors.Add("Колір квитка не збігається з кольором пачки.");
                }

                if (package.SerialId != null && package.SerialId != editDTO.SerialId)
                {
                    errors.Add("Серія квитка не збігається з серією пачки.");
                }
            }

            return errors;
        }

        public IEnumerable<string> ValidateChangeNumber(int ticketId, string newNumber)
        {
            if (!Regex.IsMatch(newNumber, @"\d{6}"))
            {
                return new string[] { "Номер повинен складатися з шести цифр. " };
            }

            var ticket = Database.Tickets.GetById(ticketId);
            var errors = new List<string>();

            if (ticket.PackageId != null)
            {
                var firstNumber = int.Parse(newNumber.First().ToString());
                var package = Database.Packages.GetById((int)ticket.PackageId);

                if (package.FirstNumber != null && package.FirstNumber != firstNumber)
                {
                    errors.Add($"Квиток повинен починатися на цифру {package.FirstNumber}.");
                }
            }

            return errors;
        }

        public IEnumerable<string> ValidateMoveToPackage(int ticketId, int packageId)
        {
            var errors = new List<string>();

            var ticket = Database.Tickets.GetById(ticketId);
            var package = Database.Packages.GetById(packageId);

            if (!package.IsOpened)
            {
                errors.Add($"Пачка \"{package.ToString()}\" закрита.");
            }

            if (package.FirstNumber != null && package.FirstNumber != int.Parse(ticket.Number.First().ToString()))
            {
                errors.Add($"До пачки \"{package.ToString()}\" можна додавати лише квитки на цифру {package.FirstNumber}.");
            }

            if (package.ColorId != null && ticket.ColorId != package.ColorId)
            {
                errors.Add("Колір квитка не збігається з кольором пачки.");
            }

            if (package.SerialId != null && ticket.SerialId != package.SerialId)
            {
                errors.Add("Серія квитка не збігається з серією пачки.");
            }

            return errors;
        }

        public IEnumerable<string> ValidateMoveFewToPackage(int packageId, params int[] ticketsIds)
        {
            var errors = new List<string>();

            var package = Database.Packages.GetById(packageId);

            if (package == null)
            {
                errors.Add($"Пачки ID: {packageId} не існує");
                return errors;
            }

            var tickets = Database.Tickets.GetAll(t => ticketsIds.Contains(t.Id)).ToList();

            if (package.FirstNumber != null && !tickets.TrueForAll(t => int.Parse(t.Number.First().ToString()) == package.FirstNumber))
            {
                errors.Add("Не всі квитки відповідають першій цифрі пачки.");
            }

            if (package.ColorId != null && !tickets.TrueForAll(t => t.ColorId == package.ColorId))
            {
                errors.Add("Не всі квитки відповідають кольору пачки.");
            }

            if (package.SerialId != null && !tickets.TrueForAll(t => t.SerialId == package.SerialId))
            {
                errors.Add("Не всі квитки відповідають серії пачки.");
            }

            return errors;
        }
    }
}
