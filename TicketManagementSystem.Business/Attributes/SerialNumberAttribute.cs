using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TicketManagementSystem.Business.Attributes
{
    public class SerialNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string serialNumber)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = "The serial number should be between 01 and 50.";
                }

                if (!Regex.IsMatch(serialNumber, @"^\d{2}$"))
                    return false;

                var toInt = int.Parse(serialNumber);

                return toInt >= 1 && toInt <= 50;
            }
            return false;
        }
    }
}
