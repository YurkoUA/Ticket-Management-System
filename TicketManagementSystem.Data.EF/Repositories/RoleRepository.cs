using System;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class RoleRepository : EFRepository<Role>
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public override Role GetByIdIncluding(int id, params Expression<Func<Role, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(r => r.Id == id);
        }
    }
}
