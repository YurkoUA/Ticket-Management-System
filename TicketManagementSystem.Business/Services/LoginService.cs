using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using Login = TicketManagementSystem.Data.EF.Models.Login;

namespace TicketManagementSystem.Business.Services
{
    public class LoginService : Service, ILoginService
    {
        const int MAX_LOGINS_BY_USER = 10;

        public LoginService(IUnitOfWork database) : base(database)
        {
        }

        public IEnumerable<LoginDTO> GetLoginHistory(int userId)
        {
            var logins = Database.Logins.GetAll(l => l.UserId == userId)
                .OrderByDescending(l => l.Id);

            return MapperInstance.Map<IEnumerable<LoginDTO>>(logins);
        }

        public IEnumerable<LoginDTO> GetLoginHistory(int userId, int take)
        {
            if (take < 1)
                throw new ArgumentOutOfRangeException();

            var logins = Database.Logins.GetAll(l => l.UserId == userId)
                .OrderByDescending(l => l.Id)
                .Take(() => take);

            return MapperInstance.Map<IEnumerable<LoginDTO>>(logins);
        }

        public void AddLogin(LoginDTO loginDTO, bool remvoveOldLogins)
        {
            var login = MapperInstance.Map<Login>(loginDTO);
            Database.Logins.Create(login);

            if (remvoveOldLogins)
                RemoveOldLogins(loginDTO.UserId);

            Database.SaveChanges();
        }

        private void RemoveOldLogins(int userId)
        {
            var logins = Database.Logins.GetAll(l => l.UserId == userId);
            var countToRemove = logins.Count() - MAX_LOGINS_BY_USER + 1;

            if (countToRemove > 0)
            {
                Database.Logins.RemoveRange(logins.Take(() => countToRemove));
            }
        }
    }
}
