using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.ViewModels.Nominal;

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
        public int NominalId { get; set; }

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

        [HiddenInput(DisplayValue = false)]
        public IEnumerable<NominalVM> Nominals { get; set; }
    }
}