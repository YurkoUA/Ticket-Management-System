using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class RoleService : Service, IRoleService
    {
        public RoleService(IUnitOfWork database) : base(database)
        {
        }

        public Task CreateAsync(Role role)
        {
            return Task.Run(() =>
            {
                Database.Roles.Create(role);
                Database.SaveChanges();
            });
        }

        public Task UpdateAsync(Role role)
        {
            return Task.Run(() =>
            {
                Database.Roles.Update(role);
                Database.SaveChanges();
            });
        }

        public Task DeleteAsync(Role role)
        {
            return Task.Run(() =>
            {
                Database.Roles.Remove(role);
                Database.SaveChanges();
            });
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            return Task.Run(() =>
            {
                return Database.Roles.GetById(roleId);
            });
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return Task.Run(() =>
            {
                return Database.Roles.GetAll(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
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
