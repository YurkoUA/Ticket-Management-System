using EvoPdf;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Business
{
    public class PdfService : IPdfService
    {
        public void CreatePdf(string url, string path)
        {
            var converter = new HtmlToPdfConverter();
            converter.ConvertUrlToFile(url, path);
        }
    }
}
