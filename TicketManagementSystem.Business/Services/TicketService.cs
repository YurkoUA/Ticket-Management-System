using System;
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
        private readonly IPackageService _packageService;
        private readonly ISerialService _serialService;
        private readonly IColorService _colorService;

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

        public int TotalCount => Database.Tickets.GetCount();

        public IEnumerable<TicketDTO> GetClones()
        {
            var clones = Database.Tickets.GetAll()
                .AsEnumerable()
                .GroupBy(t => t.Number)
                .Where(g => g.Skip(1).Any())
                .SelectMany(c => c)
                .OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(clones);
        }

        #region Get

        public IEnumerable<TicketDTO> GetTickets()
        {
            var tickets = Database.Tickets.GetAll()
                .OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTickets(int skip, int take)
        {
            var tickets = Database.Tickets.GetAll()
                .OrderBy(t => t.Number)
                //.AsEnumerable()
                .Skip(skip)
                .Take(take);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTicketsByPackage(int packageId)
        {
            var tickets = Database.Tickets.GetAll(t => t.PackageId == packageId)
                .OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets()
        {
            var tickets = Database.Tickets.GetAll(t => t.PackageId == null)
                .OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets(int packageId)
        {
            var package = Database.Packages.GetById(packageId);

            if (package == null)
                return null;

            var tickets = GetUnallocatedTickets();

            if (package.ColorId != null)
                tickets = tickets.Where(t => t.ColorId == package.ColorId);

            if (package.SerialId != null)
                tickets = tickets.Where(t => t.SerialId == package.SerialId);

            if (package.FirstNumber != null)
                tickets = tickets.Where(t => t.FirstNumber == package.FirstNumber);

            return tickets;
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets(int skip, int take)
        {
            var tickets = Database.Tickets.GetAll(t => t.PackageId == null)
                .OrderBy(t => t.Number)
                //.AsEnumerable()
                .Skip(skip)
                .Take(take);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetHappyTickets()
        {
            var tickets = Database.Tickets.GetAll()
                .OrderBy(t => t.Number)
                .AsEnumerable()
                .Where(t => t.IsHappy());

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetHappyTickets(int skip, int take)
        {
            var tickets = Database.Tickets.GetAll()
                .OrderBy(t => t.Number)
                .AsEnumerable()
                .Where(t => t.IsHappy())
                .Skip(skip)
                .Take(take);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public TicketDTO GetById(int id)
        {
            var ticket = Database.Tickets.GetById(id);

            if (ticket == null)
                return null;

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public TicketDTO GetRandomTicket()
        {
            if (TotalCount == 0)
                return null;

            var index = new Random().Next(0, TotalCount + 1);
            var ticket = Database.Tickets.GetAll()
                .OrderBy(t => t.Id)
                .Skip(index)
                .Take(1)
                .SingleOrDefault();

            if (ticket == null)
                return null;

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public IEnumerable<TicketDTO> GetByNumber(string number, bool partialMatches = false)
        {
            IEnumerable<Ticket> tickets;

            if (partialMatches)
            {
                tickets = Database.Tickets.GetAll(t => t.Number.Contains(number));
            }
            else
            {
                tickets = Database.Tickets.GetAll(t => t.Number.Equals(number));
            }

            tickets = tickets.OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetByNumber(string number, int id)
        {
            var tickets = Database.Tickets.GetAll(t => t.Number.Equals(number) && t.Id != id);
            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> Filter(int? firstNumber, int? colorId, int? serialId)
        {
            IQueryable<Ticket> tickets = Database.Tickets.GetAll();
            
            if (colorId != null)
                tickets = tickets.Where(t => t.ColorId == colorId);

            if (serialId != null)
                tickets = tickets.Where(t => t.SerialId == serialId);

            if (firstNumber != null)
                tickets = tickets.ToList().Where(t => int.Parse(t.Number.First().ToString()) == firstNumber).AsQueryable();

            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                tickets.OrderBy(t => t.Number)
            );
        }

        #endregion

        public TicketEditDTO GetEdit(int id)
        {
            var ticket = Database.Tickets.GetById(id);

            if (ticket == null)
                return null;

            var dto = MapperInstance.Map<TicketEditDTO>(ticket);

            if (ticket.PackageId != null)
            {
                dto.CanSelectColor = ticket.Package.ColorId == null;
                dto.CanSelectSerial = ticket.Package.SerialId == null;
            }
            return dto;
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
                ticket.ColorId = ticketDTO.ColorId;
                ticket.SerialId = ticketDTO.SerialId;
                ticket.SerialNumber = ticketDTO.SerialNumber;
                ticket.Note = ticketDTO.Note;
                ticket.Date = ticketDTO.Date;

                if (ticketDTO.RowVersion != null)
                    ticket.RowVersion = ticketDTO.RowVersion;

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

        public void CreateMany(TicketCreateDTO[] createDTO)
        {
            // TODO: CreateMany.
            throw new NotImplementedException();
        }

        public TicketDTO ChangeNumber(int ticketId, string number)
        {
            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null)
                return null;

            ticket.Number = number;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public TicketDTO MoveToPackage(int ticketId, int packageId)
        {
            if (!_packageService.ExistsById(packageId))
                return null;

            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null)
                return null;

            ticket.PackageId = packageId;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();

            return MapperInstance.Map<TicketDTO>(ticket);
        }

        public void MoveFewToPackage(int packageId, params int[] ticketsIds)
        {
            if (!_packageService.ExistsById(packageId))
                return;

            foreach (var id in ticketsIds)
            {
                var ticket = Database.Tickets.GetById(id);

                if (ticket != null)
                {
                    ticket.PackageId = packageId;
                }
            }
            Database.SaveChanges();
        }

        #region Count

        public int CountByNumber(string number)
        {
            return Database.Tickets.GetCount(t => t.Number.Equals(number));
        }

        public int CountByNumber(string number, int id)
        {
            return Database.Tickets.GetCount(t => t.Number.Equals(number) && t.Id != id);
        }

        public int CountUnallocatedTickets()
        {
            return Database.Tickets.GetCount(t => t.PackageId == null);
        }

        public int CountUnallocatedByPackage(int packageId)
        {
            return GetUnallocatedTickets(packageId).Count();
        }

        public int CountHappyTickets()
        {
            return Database.Tickets.GetCount(t => t.IsHappy());
        }

        #endregion

        #region Exists methods

        public bool Exists(TicketDTO ticketDTO)
        {
            return Database.Tickets.Contains(MapperInstance.Map<Ticket>(ticketDTO));
        }

        public bool Exists(TicketDTO ticketDTO, int id)
        {
            var ticket = MapperInstance.Map<Ticket>(ticketDTO);

            return Database.Tickets.Contains(t => t.Equals(ticket) && t.Id != id);
        }

        public bool ExistsById(int id)
        {
            return Database.Tickets.ExistsById(id);
        }

        public bool ExistsByNumber(string number)
        {
            return Database.Tickets.Contains(t => t.Number.Equals(number));
        }

        #endregion

        public bool CanMove(int ticketId, int packageId)
        {
            var package = _packageService.GetPackage(packageId);
            var ticket = GetById(ticketId);

            if (package != null && ticket != null)
            {
                // TODO: CanMove.
                throw new NotImplementedException();
            }
            return false;
        }

        #region Validate methods

        public IEnumerable<string> Validate(TicketCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (Exists(MapperInstance.Map<TicketDTO>(createDTO)))
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

            var dtoForCheckExisting = MapperInstance.Map<TicketDTO>(editDTO);
            dtoForCheckExisting.Number = ticket.Number;

            if (Exists(dtoForCheckExisting, editDTO.Id))
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

        #endregion
    }
}
