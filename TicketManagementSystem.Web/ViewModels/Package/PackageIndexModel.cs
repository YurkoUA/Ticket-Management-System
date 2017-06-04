using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.Web.Data;

namespace TicketManagementSystem.Web
{
    public class PackageIndexModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Колір")]
        public string ColorName => Color?.Name;

        [Display(Name = "Серія")]
        public string SerialName => Serial?.Name;

        [Display(Name = "Номінал")]
        public double Nominal { get; set; }

        [Display(Name = "Перша цифра")]
        public int? FirstNumber { get; set; }

        [Display(Name = "Статус")]
        public string Status
        {
            get
            {
                var openState = IsOpened ? "Відкрита" : "Закрита";
                var specialState = IsSpecial ? "Спеціальна" : "Звичайна";

                return $"{openState}, {specialState}";
            }
        }

        [Display(Name = "Примітка")]
        public string Note { get; set; }

        #region Hidden properties

        [HiddenInput(DisplayValue = false)]
        public PageInfo PageInfo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public TicketManagementSystem.Data.EF.Models.Color Color { get; set; }

        [HiddenInput(DisplayValue = false)]
        public TicketManagementSystem.Data.EF.Models.Serial Serial { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsOpened { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsSpecial { get; set; }

        #endregion
    }
}