using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Business.DTO.Report;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Report;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Services
{
    public class ReportService : Service, IReportService
    {
        private readonly IPackageService _packageService;
        private readonly ITicketService _ticketService;
        private readonly IPdfService _pdfService;

        public ReportService(IUnitOfWork database, ITicketService ticketServ, IPackageService packServ, IPdfService pdfService) : base(database)
        {
            _packageService = packServ;
            _ticketService = ticketServ;
            _pdfService = pdfService;
        }

        public bool IsEmpty => Database.Reports.IsEmpty();

        public IEnumerable<ReportDTO> GetReports()
        {
            var reports = Database.Reports.GetAll();
            return MapperInstance.Map<IEnumerable<ReportDTO>>(reports);
        }

        public ReportDTO GetById(int id)
        {
            var report = Database.Reports.GetById(id);

            if (report == null)
                return null;

            return MapperInstance.Map<ReportDTO>(report);
        }

        public ReportDTO GetLastReport()
        {
            var report = Database.Reports.GetAll().OrderByDescending(r => r.Id).FirstOrDefault();
            return MapperInstance.Map<ReportDTO>(report);
        }

        public ReportDTO CreateReport(bool isAutomatic)
        {
            var report = new Data.EF.Models.Report
            {
                IsAutomatic = isAutomatic
            };
            return MapperInstance.Map<ReportDTO>(report);
        }

        public void SaveReport(ReportDTO reportDTO)
        {
            Database.Reports.Create(MapperInstance.Map<Data.EF.Models.Report>(reportDTO));
            Database.SaveChanges();
        }

        public bool TryCreatePDFs(ReportDTO reportDTO, Func<string, string> actionUrl, Func<string, string> savePath)
        {
            try
            {
                Parallel.Invoke(
                    () => _pdfService.CreatePdf(actionUrl("DefaultReportPrint"), savePath(reportDTO.DefaultReportFileName())),
                    () => _pdfService.CreatePdf(actionUrl("PackagesReportPrint"), savePath(reportDTO.PackagesReportFileName()))
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public DefaultReportDTO GetDefaultReportDTO()
        {
            var lastReport = GetLastReport();

            var tickets = Database.Tickets.GetAllIncluding(t => t.Color, t => t.Serial, t => t.Package).ToList();
            var packages = _packageService.GetPackages();

            var builder = new DefaultReportBuilder();

            builder = builder.SetTicketsCount(tickets.Count())
                             .SetHappyTicketsCount(tickets.Count(t => t.Number.IsHappy()))
                             
                             .SetDefaultPackagesCount(packages.Count(p => !p.IsSpecial))

                             .SetUnallocatedTicketsCount(tickets.Count(t => t.PackageId == null));

            // Fix this. Make access through service.
            builder = builder.SetDefaultPackagesTickets(tickets.Count(t => t.Package?.IsSpecial == false));

            builder = builder.SetSpecialPackages(packages.Where(p => p.IsSpecial)
                    .Select(p => new PackageFromReportDTO
                    {
                        Id = p.Id,
                        PackageName = p.Name,
                        TotalTickets = p.TicketsCount
                    })
                    .ToList());


            if (lastReport != null)
            {
                var lastReportDate = lastReport.Date;

                builder = builder.SetLastReportDate(lastReportDate)

                                 .SetNewTicketsCount(tickets.Count(t => t.AddDate > lastReportDate))
                                 .SetNewHappyTicketsCount(tickets.Count(t => t.Number.IsHappy() && t.AddDate > lastReportDate))
                       
                                 .SetNewPackagesCount(packages.Count(t => t.Date > lastReportDate))
                       
                                 .SetNewUnallocatedTicketsCount(tickets.Count(t => t.PackageId == null && t.AddDate > lastReportDate))
                                 .SetNewDefaultPackagesTickets(tickets.Count(t => t.AddDate > lastReportDate && t.Package?.IsSpecial == false));


                // This method schould be calling from TicketService2, but we can't inject it there.
                // TODO: Use this method from TicketService2.

                builder = builder.SetNewTicketsGroups(tickets.Where(t => t.AddDate > lastReportDate)
                    .GroupBy(t => $"{t.Serial.Name}-{t.Color.Name} ({t.Number.First()})")
                    .Select(g => new TicketGroupDTO
                    {
                        Name = g.Key,
                        Count = g.Count(),
                        HappyCount = g.Count(t => t.Number.IsHappy())
                    }).OrderByDescending(t => t.Count)
                       .ToList());

                builder = builder.SetSpecialPackagesUpdates(packageId =>
                {
                    return tickets.Count(t => t.AddDate > lastReportDate && t.PackageId == packageId);
                });
            }

            return builder.Build();
        }

        public PackageReportDTO GetPackagesReportDTO()
        {
            var tickets = Database.Tickets.GetAll().ToList();

            var lastReport = GetLastReport();
            var packages = _packageService.GetPackages();

            var builder = new PackageReportBuilder()
                .SetTicketsCount(tickets.Count())
                .SetHappyTicketsCount(tickets.Count(t => t.Number.IsHappy()));

            builder = builder.SetDefaultPackages(packages.Where(p => !p.IsSpecial)
                                .GroupBy(p => p.SerialName)
                                .OrderByDescending(g => g.Count())
                                .ToDictionary(g => g.Key, g => g.AsEnumerable()
                                    .Select(p => new PackageFromReportDTO
                                    {
                                        Id = p.Id,
                                        PackageName = p.ToString(),
                                        TotalTickets = p.TicketsCount
                                    }).OrderBy(p => p.Id)
                                    .ToList()))

                            .SetSpecialPackages(packages.Where(p => p.IsSpecial)
                                .Select(p => new PackageFromReportDTO
                                {
                                    Id = p.Id,
                                    PackageName = p.ToString(),
                                    TotalTickets = p.TicketsCount
                                }).OrderBy(p => p.Id)
                                .ToList());


            if (lastReport != null)
            {
                var lastReportDate = lastReport.Date;

                builder = builder.SetLastReportDate(lastReportDate)

                                 .SetNewTicketsCount(tickets.Count(t => t.AddDate > lastReportDate))
                                 .SetNewHappyTicketsCount(tickets.Count(t => t.Number.IsHappy() && t.AddDate > lastReportDate))
                                 .SetNewPackagesCount(packages.Count(p => p.Date > lastReportDate));

                Func<int, int> func = packageId =>
                {
                    return tickets.Count(t => t.AddDate > lastReportDate && t.PackageId == packageId);
                };

                builder = builder.SetSpecialPackagesUpdates(func)
                                 .SetDefaultPackagesUpdates(func);
            }

            return builder.Build();
        }
    }
}
