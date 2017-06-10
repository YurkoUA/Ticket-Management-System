using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class PackageEditSpecialModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Timestamp]
        [HiddenInput(DisplayValue = false)]
        public byte[] RowVersion { get; set; }

        [Required(ErrorMessage = "Необхідно вказати назву пачки.")]
        [StringLength(64, MinimumLength = 2, ErrorMessage = "Назва може бути від 2 до 64 символів.")]
        public string Name { get; set; }

        [Display(Name = "Колір")]
        public int? ColorId { get; set; }

        [Display(Name = "Серія")]
        public int? SerialId { get; set; }

        [Display(Name = "Номінал")]
        [Required(ErrorMessage = "Необхідно вказати номінал пачки.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Номінал не може бути меншим за 0,1.")]
        public double Nominal { get; set; }

        [Display(Name = "Примітка")]
        [DataType(DataType.MultilineText)]
        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TicketsCount { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Colors { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Series { get; set; }
    }
}