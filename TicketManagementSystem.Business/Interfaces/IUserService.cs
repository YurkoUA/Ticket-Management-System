using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketManagementSystem.Data.EF.Models;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IUserService : IUserStore<User, int>,
                                    IUserPasswordStore<User, int>,
                                    IUserEmailStore<User, int>,
                                    IUserRoleStore<User, int>
    {
        /// <param name="login">Email or UserName</param>
        Task CreateAsync(User user, string password);
        Task ChangePasswordAsync(int userId, string password);

        Task<UserDTO> FindByLoginDataAsync(string login, string password);
        Task<UserDTO> GetUserAsync(int id);
        Task<UserDTO> GetUserAsync(string email);

        bool CheckPassword(User user, string password);
        byte[] GetUserHash(byte[] userBytes, byte[] salt);
        byte[] GetUserBytes(User user, string password);
        byte[] ComputeHash(byte[] bytes);
        byte[] GenerateSalt(int length);
    }
}
