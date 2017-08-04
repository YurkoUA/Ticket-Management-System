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
            _kernel.Bind<IPdfService>().To<PdfService>();
            _kernel.Bind<ICacheService>().To<CacheService>();

            _kernel.Bind<IUserService>().To<UserService>();
            _kernel.Bind<ILoginService>().To<LoginService>();
            _kernel.Bind<IRoleService>().To<RoleService>();

            _kernel.Bind<ISummaryService>().To<SummaryService>();
            _kernel.Bind<IReportService>().To<ReportService>();
            _kernel.Bind<ITodoService>().To<TodoService>();

            _kernel.Bind<IColorService>().To<ColorService>();
            _kernel.Bind<ISerialService>().To<SerialService>();
            _kernel.Bind<IPackageService>().To<PackageService>();
            _kernel.Bind<ITicketService>().To<TicketService>();
        }
    }
}