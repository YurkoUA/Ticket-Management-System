namespace TicketManagementSystem.Business
{
    public interface IPdfService
    {
        void CreatePdf(string url, string path);
    }
}
