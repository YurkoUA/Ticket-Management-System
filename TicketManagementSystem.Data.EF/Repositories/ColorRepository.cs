using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class ColorRepository : EFRepository<Color>
    {
        public ColorRepository(AppDbContext context) : base(context)
        {
        }

        public override IQueryable<Color> GetAllWithInclude()
        {
            return _dbSet
                .Include(c => c.Packages)
                .Include(c => c.Tickets);
        }

        public override IQueryable<Color> GetAllWithInclude(Expression<Func<Color, bool>> predicate)
        {
            return GetAllWithInclude().Where(predicate);
        }

        public override Color GetByIdWithInclude(int id)
        {
            return GetAllWithInclude(c => c.Id == id).FirstOrDefault();
        }
    }
}
