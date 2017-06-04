using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TicketManagementSystem.Web
{
    public class ColorEditModel
    {
        [Timestamp]
        [HiddenInput(DisplayValue = false)]
        public byte[] RowVersion { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool CanBeDeleted { get; set; }
    }
}