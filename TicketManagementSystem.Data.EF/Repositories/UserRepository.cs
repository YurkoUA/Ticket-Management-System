using System;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class UserRepository : EFRepository<User>
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public override User GetByIdIncluding(int id, params Expression<Func<User, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(u => u.Id == id);
        }
    }
}
