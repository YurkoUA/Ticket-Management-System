using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IRoleService : IRoleStore<Role, int>
    {
    }
}
