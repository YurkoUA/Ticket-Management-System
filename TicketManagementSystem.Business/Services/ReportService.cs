using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagementSystem.Business.DTO.Report;
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
            var reports = Database.Reports.GetAll().AsEnumerable();
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
            return MapperInstance.Map<ReportDTO>(Database.Reports.GetAll().AsEnumerable().LastOrDefault());
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
            var packages = _packageService.GetPackages();
            var tickets = _ticketService.GetTickets();

            var builder = new DefaultReportBuilder();

            builder = builder.SetTicketsCount(tickets.Count())
                             .SetHappyTicketsCount(tickets.Count(t => t.IsHappy))
                             
                             .SetDefaultPackagesCount(packages.Count(p => !p.IsSpecial))

                             .SetUnallocatedTicketsCount(tickets.Count(t => t.PackageId == null));

            // Fix this. Make access through service.
            builder = builder.SetDefaultPackagesTickets(Database.Tickets.GetCount(t => t.PackageId != null && t.Package.IsSpecial == false));

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
                                 .SetNewHappyTicketsCount(tickets.Count(t => t.IsHappy && t.AddDate > lastReportDate))
                       
                                 .SetNewPackagesCount(packages.Count(t => t.Date > lastReportDate))
                       
                                 .SetNewUnallocatedTicketsCount(tickets.Count(t => t.PackageId == null && t.AddDate > lastReportDate))
                                 .SetNewDefaultPackagesTickets(Database.Tickets.GetCount(t => t.AddDate > lastReportDate && t.PackageId != null && t.Package.IsSpecial == false));


                // This method schould be calling from TicketService2, but we can't inject it there.
                // TODO: Use this method from TicketService2.

                builder = builder.SetNewTicketsGroups(tickets.Where(t => t.AddDate > lastReportDate)
                    .GroupBy(t => $"{t.SerialName}-{t.ColorName} ({t.FirstNumber})")
                    .Select(g => new TicketGroupDTO
                    {
                        Name = g.Key,
                        Count = g.Count(),
                        HappyCount = g.Count(t => t.IsHappy)
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
            var lastReport = GetLastReport();
            var packages = _packageService.GetPackages();
            var tickets = _ticketService.GetTickets();

            var builder = new PackageReportBuilder()
                .SetTicketsCount(tickets.Count())
                .SetHappyTicketsCount(tickets.Count(t => t.IsHappy));

            builder = builder.SetDefaultPackages(packages.Where(p => !p.IsSpecial)
                                .GroupBy(p => p.SerialName)
                                .OrderByDescending(g => g.Count())
                                .ToDictionary(g => g.Key, g => g.AsEnumerable()
                                    .Select(p => new PackageFromReportDTO
                                    {
                                        Id = p.Id,
                                        PackageName = p.Name,
                                        TotalTickets = p.TicketsCount
                                    }).OrderBy(p => p.Id)
                                    .ToList()))

                            .SetSpecialPackages(packages.Where(p => p.IsSpecial)
                                .Select(p => new PackageFromReportDTO
                                {
                                    Id = p.Id,
                                    PackageName = p.Name,
                                    TotalTickets = p.TicketsCount
                                }).OrderBy(p => p.Id)
                                .ToList());


            if (lastReport != null)
            {
                var lastReportDate = lastReport.Date;

                builder = builder.SetLastReportDate(lastReportDate)

                                 .SetNewTicketsCount(tickets.Count(t => t.AddDate > lastReportDate))
                                 .SetNewHappyTicketsCount(tickets.Count(t => t.IsHappy && t.AddDate > lastReportDate))
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
