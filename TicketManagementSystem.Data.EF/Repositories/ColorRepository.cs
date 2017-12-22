using System;
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

        public override Color GetByIdIncluding(int id, params Expression<Func<Color, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(c => c.Id == id);
        }
    }
}
