using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class SerialRepository : EFRepository<Serial>
    {
        public SerialRepository(AppDbContext context) : base(context)
        {
        }

        public override IQueryable<Serial> GetAllWithInclude()
        {
            return _dbSet
                .Include(s => s.Packages)
                .Include(s => s.Tickets);
        }

        public override IQueryable<Serial> GetAllWithInclude(Expression<Func<Serial, bool>> predicate)
        {
            return GetAllWithInclude().Where(predicate);
        }

        public override Serial GetByIdWithInclude(int id)
        {
            return GetAllWithInclude(s => s.Id == id).FirstOrDefault();
        }
    }
}
