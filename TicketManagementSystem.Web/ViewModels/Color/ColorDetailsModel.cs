using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class ColorDetailsModel
    {
        [Display(Name = "ID:")]
        public int Id { get; set; }

        [Display(Name = "Назва:")]
        public string Name { get; set; }

        [Display(Name = "Пачок:")]
        public int PackagesCount { get; set; }

        [Display(Name = "Квитків:")]
        public int TicketsCount { get; set; }
    }
}