using System;
using System.Linq.Expressions;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string RoleDescription { get; set; }

        public override string ToString() => UserName;

        public static Expression<Func<User, UserDTO>> CreateFromUser = u => new UserDTO
        {
            Id = u.Id,
            Email = u.Email,
            UserName = u.UserName,
            Role = u.Role.Name,
            RoleDescription = u.Role.Description
        };
    }
}
