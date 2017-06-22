using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class TicketCreateModel
    {
        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Необхідно вказати номер квитка.")]
        [RegularExpression(@"\d{6}", ErrorMessage = "Номер повинен складатися з шести цифр.")]
        public string Number { get; set; }

        [Display(Name = "Пачка")]
        public int? PackageId { get; set; }

        [Display(Name = "Колір")]
        public int ColorId { get; set; }

        [Display(Name = "Серія")]
        public int SerialId { get; set; }

        [Display(Name = "Номер серії")]
        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [RegularExpression(@"\d{2}", ErrorMessage = "Номер серії повинен складатися з двох цифр.")]
        public string SerialNumber { get; set; }

        [Display(Name = "Примітка")]
        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [Display(Name = "Дата")]
        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Colors { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Series { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Packages { get; set; }
    }
}