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
            var packages = _packageService.GetPackages();
            var tickets = _ticketService.GetTickets();

            var dto = new DefaultReportDTO
            {
                TicketsCount = tickets.Count(),
                HappyTicketsCount = tickets.Count(t => t.IsHappy),
                SpecialPackagesCount = packages.Count(p => p.IsSpecial),
                DefaultPackagesCount = packages.Count(p => !p.IsSpecial),
                UnallocatedTicketsCount = tickets.Count(t => t.PackageId == null),

                DefaultPackagesTickets = Database.Tickets.GetCount(t => t.Package?.IsSpecial == false),

                Packages = packages.Where(p => p.IsSpecial)
                    .Select(p => new PackageDTO
                    {
                        Id = p.Id,
                        PackageName = p.Name,
                        TotalTickets = p.TicketsCount
                    })
                    .ToList()
            };

            if (reports.Any())
            {
                var lastReportDate = reports.Last().Date;
                dto.LastReportDate = lastReportDate;

                dto.NewTicketsCount = tickets.Count(t => t.AddDate > lastReportDate);
                dto.NewHappyTicketsCount = tickets.Count(t => t.IsHappy && t.AddDate > lastReportDate);

                dto.NewPackagesCount = packages.Count(t => t.Date > lastReportDate);

                dto.NewUnallocatedTicketsCount = tickets.Count(t => t.PackageId == null && t.AddDate > lastReportDate);

                dto.DefaultPackagesTicketsNew = Database.Tickets.GetCount(t => t.AddDate > lastReportDate && t.Package?.IsSpecial == false);

                dto.Packages.ForEach(p =>
                {
                    p.NewTickets = tickets.Count(t => t.AddDate > lastReportDate && t.PackageId == p.Id);
                });

                dto.NewTicketsGroups = tickets.Where(t => t.AddDate > lastReportDate)
                    .GroupBy(t => $"{t.SerialName}-{t.ColorName} ({t.FirstNumber})")
                    .Select(g => new TicketsGroup
                    {
                        Name = g.Key,
                        Count = g.Count(),
                        HappyCount = g.Count(t => t.IsHappy)
                    }).OrderByDescending(t => t.Count)
                       .ToList();
            }

            return dto;
        }

        public PackageReportDTO GetPackagesReportDTO()
        {
            // TODO: Need to clean from shit.

            var reports = Database.Reports.GetAll().AsEnumerable();
            var packages = _packageService.GetPackages();
            var tickets = _ticketService.GetTickets();

            var dto = new PackageReportDTO
            {
                TotalTickets = tickets.Count(),
                TotalPackages = packages.Count(),
                TotalDefaultPackages = packages.Count(p => !p.IsSpecial),
                HappyTickets = tickets.Count(t => t.IsHappy),

                DefaultPackages = packages.Where(p => !p.IsSpecial)
                    .GroupBy(p => p.SerialName)
                    .OrderByDescending(g => g.Count())
                    .ToDictionary(g => g.Key, g => g.AsEnumerable()
                        .Select(p => new PackageDTO
                        {
                            Id = p.Id,
                            PackageName = p.Name,
                            TotalTickets = p.TicketsCount
                        }).OrderBy(p => p.Id)
                        .ToList()),

                SpecialPackages = packages.Where(p => p.IsSpecial)
                    .Select(p => new PackageDTO
                    {
                        Id = p.Id,
                        PackageName = p.Name,
                        TotalTickets = p.TicketsCount
                    }).OrderBy(p => p.Id)
                    .ToList()
            };

            if (reports.Any())
            {
                var lastReportDate = reports.Last().Date;
                dto.LastReportDate = lastReportDate;

                Action<PackageDTO> action = pack =>
                {
                    pack.NewTickets = tickets.Count(t => t.AddDate > lastReportDate && t.PackageId == pack.Id);
                };

                dto.NewTickets = tickets.Count(t => t.AddDate > lastReportDate);
                dto.NewHappyTickets = tickets.Count(t => t.IsHappy && t.AddDate > lastReportDate);

                dto.NewPackagesCount = packages.Count(p => p.Date > lastReportDate);
                dto.SpecialPackages.ForEach(action);

                dto.DefaultPackages.Values.ToList().ForEach(v =>
                {
                    v.ForEach(action);
                });
            }

            return dto;
        }
    }
}
