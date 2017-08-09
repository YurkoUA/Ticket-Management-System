using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Extensions
{
    public static class UserExtensions
    {
        public static UserDTO ToDto(this User user)
        {
            return AutoMapperConfig.CreateMapper().Map<UserDTO>(user);
        }
    }
}
