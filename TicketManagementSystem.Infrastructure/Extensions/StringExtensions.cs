using System.Linq;

namespace TicketManagementSystem.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string FirstToUpper(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return string.Empty;
            }

            return self.First().ToString().ToUpper() + self.Substring(1);
        }
    }
}
