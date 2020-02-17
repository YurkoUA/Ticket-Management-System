using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.AutoTest.Data.Entities;

namespace TicketManagementSystem.AutoTest.Data.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>
    {
        public TicketRepository(string connectionString) : base(connectionString)
        {
        }

        public Ticket GetRandomTicket()
        {
            var sqlQuery = "SELECT TOP 1 * FROM [Ticket] ORDER BY NEWID()";
            return _context.Database.SqlQuery<Ticket>(sqlQuery).FirstOrDefault();
        }
    }
}
