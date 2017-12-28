using System.Linq;
using TicketManagementSystem.Business.Attributes;

namespace TicketManagementSystem.Business.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string str)
        {
            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        public static bool IsHappy(this string str)
        {
            var attr = new TicketNumberAttribute();

            if (!attr.IsValid(str))
                return false;

            var numbers = str.Select(n => int.Parse(n.ToString())).ToArray();
            return numbers[0] + numbers[1] + numbers[2] == numbers[3] + numbers[4] + numbers[5];
        }
    }
}
