using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class TicketMoveModel
    {
        // Hidden.
        public int Id { get; set; }       
        public string Number { get; set; }       
        public int PackageId { get; set; }

        [Display(Name = "Оберіть пачку")]
        public SelectList Packages { get; set; }
    }
}