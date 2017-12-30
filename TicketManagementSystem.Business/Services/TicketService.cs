using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Extensions;
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

        public TicketCountDTO GetCount()
        {
            var tickets = Database.Tickets.GetAll()
                .Select(t => new { t.Number, t.PackageId })
                .AsNoTracking()
                .ToList();

            return new TicketCountDTO
            {
                Total = tickets.Count(),
                Happy = tickets.Count(t => t.Number.IsHappy()),
                Unallocated = tickets.Count(t => t.PackageId == null)
            };
        }

        #region Get

        public IEnumerable<TicketDTO> GetTickets()
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Number)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTickets(int skip, int take)
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Number)
                .Skip(() => skip)
                .Take(() => take)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetTicketsByPackage(int packageId)
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.PackageId == packageId, t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Number)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets()
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.PackageId == null, t => t.Color, t => t.Serial)
                .OrderBy(t => t.Number)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
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
            var tickets = Database.Tickets.GetAllIncluding(t => t.PackageId == null, t => t.Color, t => t.Serial)
                .OrderBy(t => t.Number)
                .Skip(() => skip)
                .Take(() => take)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetHappyTickets()
        {
            var ids = GetHappyTicketsIds();
            var tickets = Database.Tickets.GetAllIncluding(t => ids.Contains(t.Id),
                    t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Number);

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetHappyTickets(int skip, int take)
        {
            var ids = GetHappyTicketsIds();
            var tickets = Database.Tickets.GetAllIncluding(t => ids.Contains(t.Id),
                    t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Number)
                .Skip(() => skip)
                .Take(() => take);

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> GetClones()
        {
            var ids = GetClonesIds();
            var clones = Database.Tickets.GetAllIncluding(t => ids.Contains(t.Id),
                    t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Number);

            return Mapper.Map<IEnumerable<TicketDTO>>(clones);
        }

        public TicketDTO GetById(int id, bool include = true)
        {
            Ticket ticket;

            if (include)
            {
                ticket = Database.Tickets.GetByIdIncluding(id, t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial);
            }
            else
            {
                ticket = Database.Tickets.GetById(id);
            }

            if (ticket == null)
                return null;

            return Mapper.Map<TicketDTO>(ticket);
        }

        public TicketDTO GetRandomTicket()
        {
            if (TotalCount == 0)
                return null;

            var index = new Random().Next(0, TotalCount + 1);

            var ticket = Database.Tickets.GetAllIncluding(t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .OrderBy(t => t.Id)
                .Skip(index)
                .Take(1)
                .SingleOrDefault();

            if (ticket == null)
                return null;

            return Mapper.Map<TicketDTO>(ticket);
        }

        public IEnumerable<TicketDTO> GetByNumber(string number, bool partialMatches = false)
        {
            IQueryable<Ticket> tickets;

            if (partialMatches)
            {
                tickets = Database.Tickets.GetAllIncluding(t => t.Number.Contains(number), t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial);
            }
            else
            {
                tickets = Database.Tickets.GetAllIncluding(t => t.Number.Equals(number), t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial);
            }

            tickets = tickets.OrderBy(t => t.Number);

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets.AsEnumerable());
        }

        public IEnumerable<TicketDTO> GetByNumber(string number, int id)
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.Number.Equals(number) && t.Id != id, t => t.Color, 
                t => t.Serial, t => t.Package, t => t.Package.Tickets, t => t.Package.Color, t => t.Package.Serial)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<TicketDTO> Filter(int? firstNumber, int? colorId, int? serialId, int skip, int take, out int count)
        {
            var tickets = Database.Tickets.GetAll();

            if (colorId != null)
                tickets = tickets.Where(t => t.ColorId == colorId);

            if (serialId != null)
                tickets = tickets.Where(t => t.SerialId == serialId);

            var sfirstNumber = firstNumber?.ToString();

            if (firstNumber != null)
                tickets = tickets.Where(t => SqlFunctions.Ascii(t.Number) == SqlFunctions.Ascii(sfirstNumber));

            tickets = tickets.OrderBy(t => t.Number);
            count = tickets.Count();

            tickets = tickets.Skip(skip).Take(take)
                .Include(t => t.Color)
                .Include(t => t.Serial)
                .Include(t => t.Package)
                .Include(t => t.Package.Tickets)
                .Include(t => t.Package.Color)
                .Include(t => t.Package.Serial);

            return Mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public TicketEditDTO GetEdit(int id)
        {
            var ticket = Database.Tickets.GetByIdIncluding(id, t => t.Package);

            if (ticket == null)
                return null;

            var dto = Mapper.Map<TicketEditDTO>(ticket);
            dto.CanSelectColor = ticket.Package?.ColorId == null;
            dto.CanSelectSerial = ticket.Package?.SerialId == null;

            return dto;
        }

        #endregion

        public TicketDTO Create(TicketCreateDTO ticketDTO)
        {
            var ticket = Database.Tickets.Create(Mapper.Map<Ticket>(ticketDTO));
            Database.SaveChanges();
            return Mapper.Map<TicketDTO>(ticket);
        }

        public void Edit(TicketEditDTO ticketDTO)
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
            }
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

        public void ChangeNumber(int ticketId, string number)
        {
            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null)
                return;

            ticket.Number = number;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();
        }

        #region Move.

        public void MoveToPackage(int ticketId, int packageId)
        {
            if (!_packageService.ExistsById(packageId))
                return;

            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null)
                return;

            ticket.PackageId = packageId;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();
        }

        public void MoveToPackage(int ticketId, int packageId, out bool isUnallocated)
        {
            isUnallocated = false;

            if (!_packageService.ExistsById(packageId))
                return;

            var ticket = Database.Tickets.GetById(ticketId);

            if (ticket == null)
                return;

            isUnallocated = ticket.PackageId == null;

            ticket.PackageId = packageId;
            Database.Tickets.Update(ticket);
            Database.SaveChanges();
        }

        public void MoveFewToPackage(int packageId, params int[] ticketsIds)
        {
            if (!_packageService.ExistsById(packageId))
                return;

            var tickets = Database.Tickets.GetAll(t => ticketsIds.Contains(t.Id));

            foreach (var t in tickets)
            {
                t.PackageId = packageId;
            }

            Database.SaveChanges();
        }

        #endregion

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
            // Need to optimize.
            return GetUnallocatedTickets(packageId).Count();
        }

        public int CountHappyTickets()
        {
            return Database.Tickets.GetAll().Select(t => t.Number)
                .AsEnumerable()
                .Count(t => t.IsHappy());
        }

        #endregion

        #region Exists methods

        public bool Exists(TicketDTO ticketDTO)
        {
            var ticket = Mapper.Map<Ticket>(ticketDTO);
            return Database.Tickets.Any(t => t.Number.Equals(ticket.Number) && t.SerialNumber.Equals(ticket.SerialNumber)
                && t.ColorId == ticket.ColorId && t.SerialId == ticket.SerialId);
        }

        public bool Exists(TicketDTO ticketDTO, int id)
        {
            var ticket = Mapper.Map<Ticket>(ticketDTO);

            return Database.Tickets.Any(t => t.Number.Equals(ticket.Number) && t.SerialNumber.Equals(ticket.SerialNumber)
                && t.ColorId == ticket.ColorId && t.SerialId == ticket.SerialId && t.Id != id);
        }

        public bool ExistsById(int id)
        {
            return Database.Tickets.ExistsById(id);
        }

        public bool ExistsByNumber(string number)
        {
            return Database.Tickets.Any(t => t.Number.Equals(number));
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

        #region GetIds.

        public IEnumerable<int> GetClonesIds()
        {
            return Database.Tickets.GetAll()
                .Select(t => new { t.Id, t.Number })
                .AsEnumerable()
                .GroupBy(t => t.Number)
                .Where(g => g.Skip(1).Any())
                .SelectMany(c => c)
                .Select(t => t.Id);
        }

        public IEnumerable<int> GetHappyTicketsIds()
        {
            return Database.Tickets.GetAll().Select(t => new
            {
                 t.Id,
                 t.Number
            }).AsEnumerable()
                .Where(t => t.Number.IsHappy())
                .Select(t => t.Id);
        }

        #endregion
    }
}
