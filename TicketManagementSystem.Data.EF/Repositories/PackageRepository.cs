using System;
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

        public override Package GetByIdIncluding(int id, params Expression<Func<Package, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(p => p.Id == id);
        }
    }
}
