using System;
using System.Data.Entity;
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

        public override IQueryable<Ticket> GetAllWithInclude()
        {
            return _dbSet
                .Include(t => t.Color)
                .Include(t => t.Serial)
                .Include(t => t.Package)
                    .Include(t => t.Package.Color)
                    .Include(t => t.Package.Serial)
                    .Include(t => t.Package.Tickets);
        }

        public override IQueryable<Ticket> GetAllWithInclude(Expression<Func<Ticket, bool>> predicate)
        {
            return GetAllWithInclude().Where(predicate);
        }

        public override Ticket GetByIdWithInclude(int id)
        {
            return GetAllWithInclude(t => t.Id == id).FirstOrDefault();
        }
    }
}
