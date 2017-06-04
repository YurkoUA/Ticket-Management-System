using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    // TODO: Make new authentication system (OWIN).
    public class AccountService : Service, IAccountService
    {
        public AccountService(IUnitOfWork database) : base(database)
        {
        }

        public User FindByPassword(string login, string password)
        {
            return Database.Users.GetAll(u => u.Password.SequenceEqual(ComputeSHA1(password)) && (u.Email.Equals(login) ||
                u.Login.Equals(login))).FirstOrDefault();
        }

        public User FindByLogin(string login)
        {
            return Database.Users.GetAll(u => u.Login.Equals(login)).FirstOrDefault();
        }

        #region private methods
        private byte[] ComputeSHA1(string password)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(Encoding.Unicode.GetBytes(password));
        }
        #endregion
    }
}
