using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class PackageDetailsModel
    {
        [Display(Name = "ID:")]
        public int Id { get; set; }

        [Display(Name = "Назва:")]
        public string Name { get; set; }

        [Display(Name = "Колір:")]
        public string ColorName { get; set; }

        [Display(Name = "Серія:")]
        public string SerialName { get; set; }

        [Display(Name = "Квитків:")]
        public int TicketsCount { get; set; }

        [Display(Name = "Номінал:")]
        public double Nominal { get; set; }

        [Display(Name = "Перша цифра:")]
        public int? FirstNumber { get; set; }

        [Display(Name = "Примітка:")]
        public string Note { get; set; }

        [Display(Name = "Статус:")]
        public string Status
        {
            get
            {
                var openState = IsOpened ? "Відкрита" : "Закрита";
                var specialState = IsSpecial ? "Спеціальна" : "Звичайна";

                return $"{openState} / {specialState}";
            }
        }

        #region Hidden properties

        [HiddenInput(DisplayValue = false)]
        public PageInfo PageInfo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsOpened { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsSpecial { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ColorId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SerialId { get; set; }

        #endregion
    }
}