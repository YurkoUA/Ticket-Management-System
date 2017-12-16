using System;
using System.Collections.Generic;
using System.Linq;
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

        public TicketDTO MoveToPackage(int ticketId, int packageId, out bool isUnallocated)
        {
            isUnallocated = false;

            if (!_packageService.ExistsById(packageId))
                return null;

            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null)
                return null;

            isUnallocated = ticket.PackageId == null;

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
            var ticket = MapperInstance.Map<Ticket>(ticketDTO);
            return Database.Tickets.Contains(t => t.Equals(ticket));
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
    }
}
