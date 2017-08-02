using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Services;

namespace TicketManagementSystem.Web.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IPdfService>().To<PdfService>().InSingletonScope();
            _kernel.Bind<ICacheService>().To<CacheService>().InSingletonScope();

            _kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            _kernel.Bind<ILoginService>().To<LoginService>().InSingletonScope();
            _kernel.Bind<IRoleService>().To<RoleService>().InSingletonScope();

            _kernel.Bind<ISummaryService>().To<SummaryService>().InSingletonScope();
            _kernel.Bind<IReportService>().To<ReportService>().InSingletonScope();
            _kernel.Bind<ITodoService>().To<TodoService>().InSingletonScope();

            _kernel.Bind<IColorService>().To<ColorService>().InSingletonScope();
            _kernel.Bind<ISerialService>().To<SerialService>().InSingletonScope();
            _kernel.Bind<IPackageService>().To<PackageService>().InSingletonScope();
            _kernel.Bind<ITicketService>().To<TicketService>().InSingletonScope();
        }
    }
}