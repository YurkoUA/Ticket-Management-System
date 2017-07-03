using Ninject;
using Quartz;
using Quartz.Impl;
using System;
using TicketManagementSystem.Web.App_Start;
using TicketManagementSystem.Web.Util;

namespace TicketManagementSystem.Web.Jobs
{
    public class JobScheduler
    {
        public static void RegisterJobs()
        {
            IKernel kernel = NinjectWebCommon.GetInstance();
            kernel.Bind<IJob>().To<SummaryJob>();

            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = new NinjectJobFactory(kernel);
            scheduler.Start();

            var job = JobBuilder.Create<SummaryJob>().Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("SummaryTrigger", "SummaryGroup")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 2, 0)
                    .InTimeZone(TimeZoneInfo.Utc))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}