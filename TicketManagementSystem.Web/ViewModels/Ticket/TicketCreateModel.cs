using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.Business.Attributes;
using TicketManagementSystem.ViewModels.Nominal;

namespace TicketManagementSystem.Web
{
    public class TicketCreateModel : IValidatableObject
    {
        [Display(Name = "Номер")]
        [Required(ErrorMessage = "Необхідно вказати номер квитка.")]
        [TicketNumber(ErrorMessage = "Номер повинен складатися з шести цифр.")]
        public string Number { get; set; }

        [Display(Name = "Пачка")]
        public int? PackageId { get; set; }

        [Display(Name = "Колір")]
        public int ColorId { get; set; }

        [Display(Name = "Серія")]
        public int SerialId { get; set; }

        [Display(Name = "Номінал")]
        public int NominalId { get; set; }

        [Display(Name = "Номер серії")]
        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [SerialNumber(ErrorMessage = "Номер серії повинен бути від 01 до 50.")]
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

        [HiddenInput(DisplayValue = false)]
        public IEnumerable<NominalVM> Nominals { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Note != null)
                Note = Note.Replace('#', '№');

            return new List<ValidationResult>();
        }
    }
}