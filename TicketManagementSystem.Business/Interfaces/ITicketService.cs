using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketService
    {
        int TotalCount { get; }

        IEnumerable<TicketDTO> GetTickets();
        IEnumerable<TicketDTO> GetTickets(int skip, int take);

        IEnumerable<TicketDTO> GetTicketsByPackage(int packageId);

        IEnumerable<TicketDTO> GetUnallocatedTickets();
        IEnumerable<TicketDTO> GetUnallocatedTickets(int skip, int take);

        IEnumerable<TicketDTO> GetHappyTickets();
        IEnumerable<TicketDTO> GetHappyTickets(int skip, int take);

        TicketDTO GetById(int id);
        IEnumerable<TicketDTO> GetByNumber(string number);
        IEnumerable<TicketDTO> GetByNumber(string number, int id);

        TicketEditDTO GetEdit(int id);

        TicketDTO Create(TicketCreateDTO ticketDTO);
        TicketDTO Edit(TicketEditDTO ticketDTO);
        void Remove(int id);

        TicketDTO ChangeNumber(int ticketId, string number);
        TicketDTO MoveToPackage(int ticketId, int packageId);

        bool ExistsById(int id);
        bool ExistsByNumber(string number);

        IEnumerable<string> Validate(TicketCreateDTO createDTO);
        IEnumerable<string> Validate(TicketEditDTO editDTO);
        IEnumerable<string> ValidateChangeNumber(int ticketId, string newNumber);
        IEnumerable<string> ValidateMoveToPackage(int ticketId, int packageId);
    }
}
