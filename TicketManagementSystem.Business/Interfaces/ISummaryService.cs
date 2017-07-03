namespace TicketManagementSystem.Business.Interfaces
{
    public interface ISummaryService
    {
        void WriteSummary();
        bool ExistsByCurrentMonth();
    }
}
