using AutoMapper;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business
{
    public abstract class Service
    {
        public IUnitOfWork Database { get; set; }
        public IMapper MapperInstance { get; }

        // TODO: TotalCount property.

        public Service(IUnitOfWork database)
        {
            Database = database;
            MapperInstance = AutoMapperConfig.CreateMapper();
        }
    }
}
