using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TicketManagementSystem.Business.Attributes
{
    public class TicketNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string number)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = "Ticket number must be consist of six digits.";
                }

                if (number.Equals("000000"))
                    return false;

                return Regex.IsMatch(number, @"^\d{6}$");
            }
            return false;
        }
    }
}
