using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
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
            _kernel.Bind<IUserService>().To<UserService>();
            _kernel.Bind<IRoleService>().To<RoleService>();
            _kernel.Bind<IColorService>().To<ColorService>();
            _kernel.Bind<ISerialService>().To<SerialService>();
            _kernel.Bind<IPackageService>().To<PackageService>();
        }
    }
}