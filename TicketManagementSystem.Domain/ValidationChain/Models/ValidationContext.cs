using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Domain.ValidationChain.Models
{
    public class ValidationContext
    {
        public IUnitOfWork unitOfWork;
    }
}
