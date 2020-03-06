using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketService
    {
        int TotalCount { get; }
        TicketCountDTO GetCount();

        IEnumerable<TicketDTO> GetTickets();
        IEnumerable<TicketDTO> GetTickets(int skip, int take);

        IEnumerable<TicketDTO> GetTicketsByPackage(int packageId);

        IEnumerable<TicketDTO> GetUnallocatedTickets();
        IEnumerable<TicketDTO> GetUnallocatedTickets(int packageId);
        IEnumerable<TicketDTO> GetUnallocatedTickets(int skip, int take);

        IEnumerable<TicketDTO> GetHappyTickets();
        IEnumerable<TicketDTO> GetHappyTickets(int skip, int take);

        IEnumerable<TicketDTO> GetClones();

        TicketDTO GetById(int id, bool include = true);

        TicketDTO GetRandomTicket();
        IEnumerable<TicketDTO> GetByNumber(string number, bool partialMatches = false);
        IEnumerable<TicketDTO> GetByNumber(string number, int id);

        IEnumerable<TicketDTO> Filter(int? firstNumber, int? colorId, int? serialId, int skip, int take, out int count);

        TicketEditDTO GetEdit(int id);

        void MoveToPackage(int ticketId, int packageId, out bool isUnallocated);
        void MoveFewToPackage(int packageId, params int[] ticketsIds);

        bool Exists(TicketDTO ticketDTO);
        bool Exists(TicketDTO ticketDTO, int id);
        bool ExistsById(int id);
        bool ExistsByNumber(string number);

        bool CanMove(int ticketId, int packageId);

        int CountByNumber(string number);
        int CountByNumber(string number, int id);

        int CountUnallocatedTickets();
        int CountUnallocatedByPackage(int packageId);
        int CountHappyTickets();

        IEnumerable<int> GetClonesIds();
        IEnumerable<int> GetHappyTicketsIds();
    }
}
