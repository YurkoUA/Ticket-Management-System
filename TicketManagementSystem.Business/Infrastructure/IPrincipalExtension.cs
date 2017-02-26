using System.Linq;
using System.Security.Principal;

namespace TicketManagementSystem.Business.Infrastructure
{
    public static class IPrincipalExtension
    {
        public static bool IsAdmin(IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return false;

            var dbUser = UnitOfWork.GetInstance().Users.GetAll(u => u.Login.Equals(user.Identity.Name)).First();

            if (dbUser == null)
                return false;

            return dbUser.Role == Data.UserRole.Admin;
        }
    }
}