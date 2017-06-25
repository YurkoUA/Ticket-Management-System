using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class TicketCreateManyModel
    {
        [Display(Name = "Пачка:")]
        public int? PackageId { get; set; }
        public string PackageName { get; set; }

        [Display(Name = "Колір")]
        public int? ColorId { get; set; }
        [Display(Name = "Серія")]
        public int? SerialId { get; set; }

        public SelectList Colors { get; set; }
        public SelectList Series { get; set; }

        public List<TicketCreateModel> Tickets { get; set; }

        public bool CanSelectColor { get; set; }
        public bool CanSelectSerial { get; set; }
    }
}