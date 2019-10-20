using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.ViewModels.Nominal;

namespace TicketManagementSystem.Web
{
    public class PackageCreateSpecialModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Необхідно вказати назву пачки.")]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "Назва може бути від 2 до 64 символів.")]
        public string Name { get; set; }
        
        [Display(Name = "Колір")]
        public int? ColorId { get; set; }
        
        [Display(Name = "Серія")]
        public int? SerialId { get; set; }

        [Display(Name = "Номінал")]
        public int? NominalId { get; set; }

        [Display(Name = "Примітка")]
        [DataType(DataType.MultilineText)]
        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Colors { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Series { get; set; }

        [HiddenInput(DisplayValue = false)]
        public IEnumerable<NominalVM> Nominals { get; set; }
    }
}