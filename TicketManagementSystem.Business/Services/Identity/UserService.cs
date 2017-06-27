using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class UserService : Service, IUserService
    {
        private IRoleService _roleService;

        public UserService(IUnitOfWork database, IRoleService roleService) : base(database)
        {
            _roleService = roleService;
        }

        public async Task<UserDTO> FindByLoginDataAsync(string login, string password)
        {
            var user = await FindByEmailAsync(login) ?? await FindByNameAsync(login);

            if (user != null)
            {
                if (CheckPassword(user, password))
                {
                    return MapperInstance.Map<UserDTO>(user);
                }
            }
            return null;
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            var user = await FindByIdAsync(id);

            if (user == null)
                return null;

            return MapperInstance.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserAsync(string email)
        {
            var user = await FindByEmailAsync(email);

            if (user == null)
                return null;

            return MapperInstance.Map<UserDTO>(user);
        }

        public Task CreateAsync(User user)
        {
            return Task.Run(() =>
            {
                Database.Users.Create(user);
                Database.SaveChanges();
            });
        }

        /// <summary>
        /// Creating user within password and default role.
        /// </summary>
        public async Task CreateAsync(User user, string password)
        {
            await SetPasswordHashAsync(user, password);
            await AddToRoleAsync(user, "User");
            await CreateAsync(user);
        }

        public async Task ChangePasswordAsync(int userId, string password)
        {
            var user = await FindByIdAsync(userId);

            if (user != null)
            {
                await SetPasswordHashAsync(user, password);
                Database.SaveChanges();
            }
        }

        public Task UpdateAsync(User user)
        {
            return Task.Run(() =>
            {
                Database.Users.Update(user);
                Database.SaveChanges();
            });
        }

        public Task DeleteAsync(User user)
        {
            return Task.Run(() =>
            {
                Database.Users.Remove(user);
                Database.SaveChanges();
            });
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            // User can be relate to only one role.
            var role = await _roleService.FindByNameAsync(roleName);

            if (role != null)
            {
                user.RoleId = role.Id;
                Database.SaveChanges();
            }
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Run(() =>
            {
                return Database.Users.GetById(userId);
            });
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                return Database.Users.GetAll(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                    .FirstOrDefault();
            });
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Task.Run(() =>
            {
                return Database.Users.GetAll(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                    .FirstOrDefault();
            }); ;
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.Run(() =>
            {
                return user.Email;
            });
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            // Email is always confirmed.
            return Task.Run(() => true);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.Run(() =>
            {
                // TODO: Byte array to string.
                return user.PasswordHash.ToString();
            });
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            // TODO: Check for null.
            var role = await _roleService.FindByIdAsync(user.RoleId);
            return new string[] { role.ToString() }.ToList();
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            // TODO: HasPasswordAsync.
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var role = await _roleService.FindByNameAsync(roleName);

            if (role != null)
            {
                return user.Role.Equals(role);
            }
            return false;
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            // TODO: RemoveFromRoleAsync.
            // User can be relate to only one role.
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string email)
        {
            // TODO: Change hash after change email.
            return Task.Run(() =>
            {
                user.Email = email;
            });
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WARNING!!! Without saving database.
        /// </summary>
        /// <param name="passwordHash">This is not a hash, this is original password.</param>
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.Run(() =>
            {
                var password = passwordHash;
                var userBytes = GetUserBytes(user, password);
                var salt = GenerateSalt(userBytes.Length);

                user.PasswordHash = GetUserHash(userBytes, salt);
                user.Salt = salt;
            });
        }

        public bool CheckPassword(User user, string password)
        {
            try
            {
                var hash = GetUserHash(GetUserBytes(user, password), user.Salt);
                return user.PasswordHash.SequenceEqual(hash);
            }
            catch
            {
                return false;
            }
        }

        public byte[] GetUserHash(byte[] userBytes, byte[] salt)
        {
            var userBits = new BitArray(userBytes.ToArray());
            var saltBits = new BitArray(salt);

            var xorResult = userBits.Xor(saltBits);

            var identitySum = new byte[userBytes.Length];
            xorResult.CopyTo(identitySum, 0);

            return ComputeHash(identitySum);
        }

        public byte[] GetUserBytes(User user, string password)
        {
            var userBytes = new List<byte>();
            addBytes(password, user.Email, user.UserName);

            return userBytes.ToArray();

            void addBytes(params string[] str)
            {
                foreach (var s in str)
                    userBytes.AddRange(Encoding.UTF8.GetBytes(s));
            }
        }

        public byte[] ComputeHash(byte[] bytes)
        {
            byte[] hash;

            using (var crypter = new SHA512CryptoServiceProvider())
            {
                hash = crypter.ComputeHash(bytes);
            }

            return hash;
        }

        public byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(salt);
            }

            return salt;
        }

        public void Dispose()
        {
            // TODO: Dispose.
            throw new NotImplementedException();
        }
    }
}
