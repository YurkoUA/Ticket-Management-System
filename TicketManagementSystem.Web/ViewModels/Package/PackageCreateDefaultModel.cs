using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class PackageCreateDefaultModel
    {
        [Required(ErrorMessage = "Необхідно обрати колір.")]
        [Display(Name = "Колір")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "Необхідно обрати серію.")]
        [Display(Name = "Серія")]
        public int SerialId { get; set; }

        [Display(Name = "Номінал")]
        [Required(ErrorMessage = "Необхідно вказати номінал пачки.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Номінал не може бути меншим за 0,1.")]
        public double Nominal { get; set; }

        [Display(Name = "Перша цифра")]
        [Range(0, 9, ErrorMessage = "Повинно бути число від 0 до 9.")]
        public int? FirstNumber { get; set; }

        [Display(Name = "Примітка")]
        [DataType(DataType.MultilineText)]
        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Colors { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Series { get; set; }
    }
}