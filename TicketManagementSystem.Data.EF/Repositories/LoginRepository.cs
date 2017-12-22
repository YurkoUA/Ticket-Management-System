using System;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class LoginRepository : EFRepository<Login>
    {
        public LoginRepository(AppDbContext context) : base(context)
        {
        }

        public override Login GetByIdIncluding(int id, params Expression<Func<Login, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(l => l.Id == id);
        }
    }
}
