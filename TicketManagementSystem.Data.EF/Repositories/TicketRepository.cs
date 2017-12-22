using System;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class TicketRepository : EFRepository<Ticket>
    {
        public TicketRepository(AppDbContext context) : base(context)
        {
        }

        public override Ticket GetByIdIncluding(int id, params Expression<Func<Ticket, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(t => t.Id == id);
        }
    }
}
