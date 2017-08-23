using System.Linq;

namespace TicketManagementSystem.Business.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string str)
        {
            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
}
