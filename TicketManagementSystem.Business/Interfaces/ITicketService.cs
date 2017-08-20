using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITicketService
    {
        int TotalCount { get; }

        IEnumerable<TicketDTO> GetClones();

        IEnumerable<TicketDTO> GetTickets();
        IEnumerable<TicketDTO> GetTickets(int skip, int take);

        IEnumerable<TicketDTO> GetTicketsByPackage(int packageId);

        IEnumerable<TicketDTO> GetUnallocatedTickets();
        IEnumerable<TicketDTO> GetUnallocatedTickets(int packageId);
        IEnumerable<TicketDTO> GetUnallocatedTickets(int skip, int take);

        IEnumerable<TicketDTO> GetHappyTickets();
        IEnumerable<TicketDTO> GetHappyTickets(int skip, int take);

        TicketDTO GetById(int id);
        IEnumerable<TicketDTO> GetByNumber(string number, bool partialMatches = false);
        IEnumerable<TicketDTO> GetByNumber(string number, int id);

        IEnumerable<TicketDTO> Filter(int? firstNumber, int? colorId, int? serialId);

        TicketEditDTO GetEdit(int id);

        TicketDTO Create(TicketCreateDTO ticketDTO);
        TicketDTO Edit(TicketEditDTO ticketDTO);
        void Remove(int id);

        void CreateMany(TicketCreateDTO[] createDTO);

        TicketDTO ChangeNumber(int ticketId, string number);
        TicketDTO MoveToPackage(int ticketId, int packageId);
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

        IEnumerable<string> Validate(TicketCreateDTO createDTO);
        IEnumerable<string> Validate(TicketEditDTO editDTO);
        IEnumerable<string> ValidateChangeNumber(int ticketId, string newNumber);
        IEnumerable<string> ValidateMoveToPackage(int ticketId, int packageId);
        IEnumerable<string> ValidateMoveFewToPackage(int packageId, params int[] ticketsIds);
    }
}
