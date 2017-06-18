using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class TicketService : Service, ITicketService
    {
        private IPackageService _packageService;
        private ISerialService _serialService;
        private IColorService _colorService;

        public TicketService
            (IUnitOfWork database, 
            IPackageService packageService, 
            ISerialService serialService, 
            IColorService colorService) : base(database)
        {
            _packageService = packageService;
            _serialService = serialService;
            _colorService = colorService;
        }

        #region Get

        public int TotalCount => Database.Tickets.GetCount();

        public IEnumerable<TicketDTO> GetTickets()
        {
            var tickets = Database.Tickets.GetAll()
                .OrderBy(t => t.Number);
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTickets(int skip, int take)
        {
            var tickets = Database.Tickets.GetAll().AsEnumerable().Skip(skip).Take(take)
                .OrderBy(t => t.Number);
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTicketsByPackage(int packageId)
        {
            var tickets = Database.Tickets.GetAll(t => t.PackageId == packageId);
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets()
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.PackageId == null)
                .OrderBy(t => t.Number)
                .AsEnumerable());
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets(int skip, int take)
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.PackageId == null)
                .AsEnumerable()
                .Skip(skip)
                .Take(take)
                .OrderBy(t => t.Number));
        }

        public IEnumerable<TicketDTO> GetHappyTickets()
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.IsHappy())
                .AsEnumerable()
                .OrderBy(t => t.Number));
        }

        public IEnumerable<TicketDTO> GetHappyTickets(int skip, int take)
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.IsHappy())
                .AsEnumerable()
                .Skip(skip)
                .Take(take)
                .OrderBy(t => t.Number));
        }

        public TicketDTO GetById(int id)
        {
            var ticket = Database.Tickets.GetById(id);

            if (ticket == null) return null;

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public IEnumerable<TicketDTO> GetByNumber(string number)
        {
            var tickets = Database.Tickets.GetAll().Where(t => t.Number.Equals(number));
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetByNumber(string number, int id)
        {
            var tickets = Database.Tickets.GetAll().Where(t => t.Number.Equals(number) && t.Id != id);
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        #endregion

        public TicketEditDTO GetEdit(int id)
        {
            var ticket = Database.Tickets.GetById(id);

            if (ticket == null)
                return null;

            return MapperInstance.Map<TicketEditDTO>(ticket);
        }

        public TicketDTO Create(TicketCreateDTO ticketDTO)
        {
            var ticket = Database.Tickets.Create(MapperInstance.Map<Ticket>(ticketDTO));
            Database.SaveChanges();
            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public TicketDTO Edit(TicketEditDTO ticketDTO)
        {
            var ticket = Database.Tickets.GetById(ticketDTO.Id);

            if (ticket != null)
            {
                ticket.RowVersion = ticketDTO.RowVersion;
                ticket.ColorId = ticketDTO.ColorId;
                ticket.SerialId = ticketDTO.SerialId;
                ticket.SerialNumber = ticketDTO.SerialNumber;
                ticket.Note = ticketDTO.Note;
                ticket.Date = ticketDTO.Date;

                Database.Tickets.Update(ticket);
                Database.SaveChanges();

                return MapperInstance.Map<TicketDTO>(ticket);
            }
            return null;
        }

        public void Remove(int id)
        {
            var ticket = Database.Tickets.GetById(id);

            if (ticket != null)
            {
                Database.Tickets.Remove(ticket);
                Database.SaveChanges();
            }
        }

        public TicketDTO ChangeNumber(int ticketId, string number)
        {
            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null) return null;

            ticket.Number = number;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public TicketDTO MoveToPackage(int ticketId, int packageId)
        {
            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null) return null;

            ticket.PackageId = packageId;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public bool ExistsById(int id)
        {
            return Database.Tickets.ExistsById(id);
        }

        public bool ExistsByNumber(string number)
        {
            return Database.Tickets.Contains(t => t.Number.Equals(number));
        }

        public IEnumerable<string> Validate(TicketCreateDTO createDTO)
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

            if (createDTO.PackageId != null)
            {
                var package = _packageService.GetPackage((int)createDTO.PackageId);

                if (package == null)
                {
                    errors.Add($"Пачки ID: {createDTO.PackageId} не існує.");
                }
                else if (!package.IsOpened)
                {
                    errors.Add($"Пачка \"{package.Name}\" закрита.");
                }
                else if (package.FirstNumber != null && package.FirstNumber != int.Parse(createDTO.Number.First().ToString()))
                {
                    errors.Add($"До пачки \"{package.Name}\" можна додавати лише квитки на цифру {package.FirstNumber}.");
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

            var ticketDTO = GetById(editDTO.Id);

            if (!_colorService.ExistsById(editDTO.ColorId))
            {
                errors.Add($"Кольору ID: {editDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(editDTO.SerialId))
            {
                errors.Add($"Серії ID: {editDTO.SerialId} не існує.");
            }

            if (ticketDTO.PackageId != null)
            {
                var packageDTO = _packageService.GetPackage((int)ticketDTO.PackageId);

                if (packageDTO.ColorId != null && packageDTO.ColorId != editDTO.ColorId)
                {
                    errors.Add("Колір квитка не збігається з кольором пачки.");
                }

                if (packageDTO.SerialId != null && packageDTO.SerialId != editDTO.SerialId)
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
                return new string[] { "Номер повинен складатися з шести цифр. "};
            }

            var ticketDTO = GetById(ticketId);
            var errors = new List<string>();

            if (ticketDTO.PackageId != null)
            {
                var firstNumber = int.Parse(newNumber.First().ToString());
                var packageDTO = _packageService.GetPackage((int)ticketDTO.PackageId);

                if (packageDTO.FirstNumber != null && packageDTO.FirstNumber != firstNumber)
                {
                    errors.Add($"Квиток повинен починатися на цифру {packageDTO.FirstNumber}.");
                }
            }

            return errors;
        }

        public IEnumerable<string> ValidateMoveToPackage(int ticketId, int packageId)
        {
            var errors = new List<string>();
            var ticketDTO = GetById(ticketId);
            var packageDTO = _packageService.GetPackage(packageId);

            if (!packageDTO.IsOpened)
            {
                errors.Add($"Пачка \"{packageDTO.Name}\" закрита.");
            }

            if (packageDTO.FirstNumber != null && packageDTO.FirstNumber != int.Parse(ticketDTO.Number.First().ToString()))
            {
                errors.Add($"До пачки \"{packageDTO.Name}\" можна додавати лише квитки на цифру {packageDTO.FirstNumber}.");
            }

            if (packageDTO.ColorId != null && ticketDTO.ColorId != packageDTO.ColorId)
            {
                errors.Add("Колір квитка не збігається з кольором пачки.");
            }

            if (packageDTO.SerialId != null && ticketDTO.SerialId != packageDTO.SerialId)
            {
                errors.Add("Серія квитка не збігається з серією пачки.");
            }

            return errors;
        }
    }
}
