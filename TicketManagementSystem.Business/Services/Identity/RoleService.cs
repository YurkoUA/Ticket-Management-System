using System;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.Services
{
    public class RoleService : Service, IRoleService
    {
        public RoleService(IUnitOfWork database) : base(database)
        {
        }

        public async Task CreateAsync(Role role)
        {
            await Task.Run(() =>
            {
                Database.Roles.Create(role);
                Database.SaveChanges();
            });
        }

        public async Task UpdateAsync(Role role)
        {
            await Task.Run(() =>
            {
                Database.Roles.Update(role);
                Database.SaveChanges();
            });
        }

        public async Task DeleteAsync(Role role)
        {
            await Task.Run(() =>
            {
                Database.Roles.Remove(role);
                Database.SaveChanges();
            });
        }

        public async Task<Role> FindByIdAsync(int roleId)
        {
            return await Task.Run(() => Database.Roles.GetByIdIncluding(roleId, r => r.Users));
        }

        public async Task<Role> FindByNameAsync(string roleName)
        {
            return await Task.Run(() =>
            {
                return Database.Roles.GetAllIncluding(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase), r => r.Users)
                    .FirstOrDefault();
            });
        }

        public void Dispose()
        {
            // TODO: Dispose.
            throw new NotImplementedException();
        }
    }
}
