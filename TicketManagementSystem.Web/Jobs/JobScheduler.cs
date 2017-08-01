using Quartz;
using Quartz.Impl;

namespace TicketManagementSystem.Web.Jobs
{
    public class JobScheduler
    {
        public static void RegisterJobs(string reportsDirectory, string baseUrl)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            StartSummaryJob(scheduler);
            StartReportJob(scheduler, reportsDirectory, baseUrl);
        }

        private static void StartSummaryJob(IScheduler scheduler)
        {
            var job = JobBuilder.Create<SummaryJob>()
                .Build();

#if DEBUG
            var trigger = TriggerBuilder.Create()
                .WithIdentity("SummaryTrigger", "SummaryGroup")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(20)
                    .RepeatForever())
                .Build();
#else

            var trigger = TriggerBuilder.Create()
                .WithIdentity("SummaryTrigger", "SummaryGroup")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 2, 0)
                    .InTimeZone(TimeZoneInfo.Utc))
                .Build();

#endif

            scheduler.ScheduleJob(job, trigger);
        }

        private static void StartReportJob(IScheduler scheduler, string reportsDirectory, string baseUrl)
        {
            var job = JobBuilder.Create<ReportJob>()
                .Build();

            job.JobDataMap["reportsDirectory"] = reportsDirectory;
            job.JobDataMap["baseUrl"] = baseUrl;

#if DEBUG
            var trigger = TriggerBuilder.Create()
                .WithIdentity("ReportTrigger", "ReportGroup")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(20)
                    .RepeatForever())
                .Build();
#else
            var trigger = TriggerBuilder.Create()
                .WithIdentity("ReportTrigger", "ReportGroup")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 2, 2)
                    .InTimeZone(TimeZoneInfo.Utc))
                .Build();
#endif

            scheduler.ScheduleJob(job, trigger);
        }
    }
}