using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ILoginService
    {
        IEnumerable<LoginDTO> GetLoginHistory(int userId);
        IEnumerable<LoginDTO> GetLoginHistory(int userId, int take);
        void AddLogin(LoginDTO loginDTO, bool removeOldLogins);
    }
}
