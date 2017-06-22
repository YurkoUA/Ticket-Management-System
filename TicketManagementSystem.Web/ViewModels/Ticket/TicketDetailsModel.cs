using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class TicketDetailsModel
    {
        [Display(Name = "ID:")]
        public int Id { get; set; }
        [Display(Name = "Номер:")]
        public string Number { get; set; }

        public int? PackageId { get; set; }
        [Display(Name = "Пачка:")]
        public string PackageName { get; set; }

        public int ColorId { get; set; }
        [Display(Name = "Колір:")]
        public string ColorName { get; set; }

        public int SerialId { get; set; }

        public string SerialName { get; set; }
        public string SerialNumber { get; set; }

        [Display(Name = "Серія:")]
        public string SerialFull => SerialName + SerialNumber;

        [Display(Name = "Примітка:")]
        public string Note { get; set; }

        [Display(Name = "Дата:")]
        public string Date { get; set; }
        [Display(Name = "Додано:")]
        public string AddDate { get; set; }

        [Display(Name = "Щасливий:")]
        public bool IsHappy { get; set; }

        public int? Clones { get; set; }

        public string TrClass()
        {
            return IsHappy ? "success" : "";
        }
    }
}