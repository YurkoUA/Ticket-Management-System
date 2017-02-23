using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Business
{
    public abstract class Service<T> where T : class
    {
        protected UnitOfWork _uow = UnitOfWork.GetInstance();
    }
}
