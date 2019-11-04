using System.Data.Entity;

namespace TicketManagementSystem.Data
{
    public static class SqlEntityExtensions
    {
        public static string SqlFirst(this string self)
        {
            return DbFunctions.Left(self, 1);
        }
    }
}
