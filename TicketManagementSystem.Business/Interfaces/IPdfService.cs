namespace TicketManagementSystem.Business.Interfaces
{
    public interface IPdfService
    {
        void CreatePdf(string url, string path);
    }
}
