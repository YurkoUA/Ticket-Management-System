using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using TicketManagementSystem.Business.Infrastructure;
using TicketManagementSystem.Domain.Util;
using TicketManagementSystem.Web.Util;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TicketManagementSystem.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(TicketManagementSystem.Web.App_Start.NinjectWebCommon), "Stop")]

namespace TicketManagementSystem.Web.App_Start
{
    public static class NinjectWebCommon
    {
        private static IKernel kernel;
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        public static IKernel GetInstance()
        {
            return kernel;
        }
        
        private static IKernel CreateKernel()
        {
            var modules = new INinjectModule[] 
            {
                new ServiceModule("DefaultConnection"),
                new DomainModule("DefaultConnection")
            };
            kernel = new StandardKernel(modules);

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            System.Web.Mvc.DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Ninject.Web.WebApi.NinjectDependencyResolver(kernel);
        }        
    }
}
