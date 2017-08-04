using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class SelectListModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SelectList Options { get; set; }
        public string DefaultOption { get; set; }
    }
}