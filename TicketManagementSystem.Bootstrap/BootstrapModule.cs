using Ninject.Modules;
using TicketManagementSystem.Bootstrap.Mapping;
using TicketManagementSystem.Infrastructure.Util;

namespace TicketManagementSystem.Bootstrap
{
    public class BootstrapModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEntityService>().To<EntityConverter>();
        }
    }
}
