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
        public TicketService(IUnitOfWork database) : base(database)
        {
        }

        #region Get

        public int TotalCount => Database.Tickets.GetCount();

        public IEnumerable<TicketDTO> GetTickets()
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(Database.Tickets.GetAll());
        }

        public IEnumerable<TicketDTO> GetTickets(int skip, int take)
        {
            var tickets = Database.Tickets.GetAll().AsEnumerable().Skip(skip).Take(take);
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
                .AsEnumerable());
        }

        public IEnumerable<TicketDTO> GetUnallocatedTickets(int skip, int take)
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.PackageId == null)
                .AsEnumerable()
                .Skip(skip)
                .Take(take));
        }

        public IEnumerable<TicketDTO> GetHappyTickets()
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.IsHappy())
                .AsEnumerable());
        }

        public IEnumerable<TicketDTO> GetHappyTickets(int skip, int take)
        {
            return MapperInstance.Map<IEnumerable<TicketDTO>>(
                Database.Tickets.GetAll()
                .Where(t => t.IsHappy())
                .AsEnumerable()
                .Skip(skip)
                .Take(take));
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
    }
}
