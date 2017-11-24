using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Ninject;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Business.Telegram;

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
            BindTelegramService();

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
            _kernel.Bind<ITicketService2>().To<TicketService2>();
        }

        private void BindTelegramService()
        {
            bool isEnabled;

            bool.TryParse(ConfigurationManager.AppSettings["IsTelegramNotificationsEnabled"],
                out isEnabled);

            var notificationsSettings = new TelegramNotificationsSettings
            {
                AccessToken = ConfigurationManager.AppSettings["BotToken"],
                ChatId = ConfigurationManager.AppSettings["TelegramChatId"],
                IsNotificationsEnabled = isEnabled
            };

            _kernel.Bind<ITelegramNotificationService>().To<TelegramNotificationService>()
                .InSingletonScope()
                .WithConstructorArgument(notificationsSettings);

            _kernel.Bind<ITelegramService>().To<TelegramService>()
                .InSingletonScope();
        }
    }
}