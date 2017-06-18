using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class PackageMakeDefaultModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Колір")]
        public int ColorId { get; set; }

        [Display(Name = "Серія")]
        public int SerialId { get; set; }

        [Display(Name = "Перша цифра")]
        [Range(0, 9, ErrorMessage = "Повинно бути число від 0 до 9.")]
        public int? FirstNumber { get; set; }

        public SelectList Colors { get; set; }
        public SelectList Series { get; set; }
    }
}