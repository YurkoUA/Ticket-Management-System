using System;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Data.EF.Repositories
{
    public class SerialRepository : EFRepository<Serial>
    {
        public SerialRepository(AppDbContext context) : base(context)
        {
        }

        public override Serial GetByIdIncluding(int id, params Expression<Func<Serial, object>>[] includeProperties)
        {
            return base.GetAllIncluding(includeProperties).SingleOrDefault(s => s.Id == id);
        }
    }
}
