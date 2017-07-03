using Ninject;
using Ninject.Syntax;
using Quartz.Simpl;
using Quartz;
using Quartz.Spi;

namespace TicketManagementSystem.Web.Util
{
    public class NinjectJobFactory : SimpleJobFactory
    {
        private IResolutionRoot _resolutionRoot;

        public NinjectJobFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _resolutionRoot.Get<IJob>();
        }
    }
}