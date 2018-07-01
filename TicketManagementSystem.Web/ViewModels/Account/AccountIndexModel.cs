using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class AccountIndexModel
    {
        [Display(Name = "ID:")]
        public int Id { get; set; }

        [Display(Name = "Email:")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Ім'я:")]
        public string UserName { get; set; }

        [Display(Name = "Група:")]
        public string RoleDescription { get; set; }
    }
}