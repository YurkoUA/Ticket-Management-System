using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Interfaces
{
    // Implement interfaces from Identity.
    public interface IAccountService
    {
        User FindByLogin(string login);
        User FindByPassword(string login, string password);
    }
}