using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO.Report;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class ReportService : Service, IReportService
    {
        private IPackageService _packageService;
        private ITicketService _ticketService;

        public ReportService(IUnitOfWork database, ITicketService ticketServ, IPackageService packServ) : base(database)
        {
            _packageService = packServ;
            _ticketService = ticketServ;
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
            var report = new Report
            {
                IsAutomatic = isAutomatic
            };
            return MapperInstance.Map<ReportDTO>(report);
        }

        public void SaveReport(ReportDTO reportDTO)
        {
            Database.Reports.Create(MapperInstance.Map<Report>(reportDTO));
            Database.SaveChanges();
        }

        public DefaultReportDTO GetDefaultReportDTO()
        {
            // TODO: Need to clean from shit.

            var reports = Database.Reports.GetAll().AsEnumerable();

            var dto = new DefaultReportDTO
            {
                TicketsCount = _ticketService.TotalCount,
                HappyTicketsCount = _ticketService.CountHappyTickets(),
                SpecialPackagesCount = Database.Packages.GetCount(p => p.IsSpecial),
                DefaultPackagesCount = Database.Packages.GetCount(p => !p.IsSpecial),
                DefaultPackagesTickets = Database.Tickets.GetCount(t => t.Package?.IsSpecial == false),
                UnallocatedTicketsCount = Database.Tickets.GetCount(t => t.PackageId == null),

                Packages = Database.Packages.GetAll(p => p.IsSpecial)
                    .Select(p => new PackageDTO
                    {
                        Id = p.Id,
                        PackageName = p.ToString(),
                        TotalTickets = p.Tickets.Count()
                    })
                    .ToList()
            };

            if (reports.Any())
            {
                var lastReportDate = reports.Last().Date;
                dto.LastReportDate = lastReportDate;

                dto.NewTicketsCount = Database.Tickets.GetCount(t => t.AddDate > lastReportDate);
                dto.NewPackagesCount = Database.Packages.GetCount(t => t.Date > lastReportDate);
                dto.DefaultPackagesTicketsNew = Database.Tickets.GetCount(t => t.AddDate > lastReportDate && t.Package?.IsSpecial == false);
                dto.NewUnallocatedTicketsCount = Database.Tickets.GetCount(t => t.PackageId == null && t.AddDate > lastReportDate);

                dto.Packages.ForEach(p =>
                {
                    var package = Database.Packages.GetById(p.Id);
                    p.NewTickets = package.Tickets.Count(t => t.AddDate > lastReportDate);
                });
            }

            return dto;
        }

        public PackageReportDTO GetPackagesReportDTO()
        {
            // TODO: Need to clean from shit.

            var reports = Database.Reports.GetAll().AsEnumerable();
            var packages = Database.Packages.GetAll().AsEnumerable();

            var dto = new PackageReportDTO
            {
                TotalTickets = _ticketService.TotalCount,
                TotalPackages = _packageService.TotalCount,
                HappyTickets = _ticketService.CountHappyTickets(),

                DefaultPackages = packages.Where(p => !p.IsSpecial)
                    .Select(p => new PackageDTO
                    {
                        Id = p.Id,
                        PackageName = p.ToString(),
                        TotalTickets = p.Tickets.Count()
                    })
                    .ToList(),

                SpecialPackages = packages.Where(p => p.IsSpecial)
                    .Select(p => new PackageDTO
                    {
                        Id = p.Id,
                        PackageName = p.ToString(),
                        TotalTickets = p.Tickets.Count()
                    })
                    .ToList()
            };

            if (reports.Any())
            {
                var lastReportDate = reports.Last().Date;
                dto.LastReportDate = lastReportDate;

                Action<PackageDTO> action = pack =>
                {
                    var package = Database.Packages.GetById(pack.Id);
                    pack.NewTickets = package.Tickets.Count(t => t.AddDate > lastReportDate);
                };

                dto.NewTickets = Database.Tickets.GetCount(t => t.AddDate > lastReportDate);
                dto.NewPackagesCount = Database.Packages.GetCount(p => p.Date > lastReportDate);
                dto.DefaultPackages.ForEach(action);
                dto.SpecialPackages.ForEach(action);
            }

            return dto;
        }
    }
}
