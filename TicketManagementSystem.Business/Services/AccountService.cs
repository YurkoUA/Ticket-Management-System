using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TicketManagementSystem.Data.Models;

namespace TicketManagementSystem.Business.Services
{
    public class AccountService : Service<User>
    {
        private AccountService()
        {
            _repo = _uow.Users;
        }

        #region Singleton implementation
        private static AccountService _serviceSingleton;

        public static AccountService GetInstance()
        {
            if (_serviceSingleton == null)
                _serviceSingleton = new AccountService();

            return _serviceSingleton;
        }
        #endregion

        public User FindByPassword(string login, string password)
        {
            return _repo.GetAll(u => u.Password.SequenceEqual(ComputeSHA1(password)) && (u.Email.Equals(login) ||
                u.Login.Equals(login))).FirstOrDefault();
        }

        public User FindByLogin(string login)
        {
            return _repo.GetAll(u => u.Login.Equals(login)).FirstOrDefault();
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
