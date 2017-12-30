using AutoMapper;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business
{
    public abstract class Service
    {
        public IUnitOfWork Database { get; set; }
        public IMapper Mapper { get; }

        public Service(IUnitOfWork database)
        {
            Database = database;
            Mapper = AutoMapperConfig.GetInstance();
        }
    }
}
