using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ILoginService
    {
        IEnumerable<LoginDTO> GetLoginHistory(int userId);
        void AddLogin(LoginDTO loginDTO);
    }
}
