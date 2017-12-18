using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class PackageRepository : EFRepository<Package>
    {
        public PackageRepository(AppDbContext context) : base(context)
        {
        }

        public override IQueryable<Package> GetAllWithInclude()
        {
            return _dbSet
                .Include(p => p.Color)
                .Include(p => p.Serial)
                .Include(p => p.Tickets);
            
        }

        public override IQueryable<Package> GetAllWithInclude(Expression<Func<Package, bool>> predicate)
        {
            return GetAllWithInclude().Where(predicate);
        }

        public override Package GetByIdWithInclude(int id)
        {
            return GetAllWithInclude(p => p.Id == id).FirstOrDefault();
        }
    }
}
