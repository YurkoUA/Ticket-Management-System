using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO.Report;

namespace TicketManagementSystem.Business.Report
{
    public class PackageReportBuilder : ReportBuilder<PackageReportDTO, PackageReportBuilder>
    {
        public PackageReportBuilder()
        {
            _reportDto = new PackageReportDTO();
        }

        public PackageReportBuilder SetDefaultPackages(Dictionary<string, List<PackageFromReportDTO>> packages)
        {
            _reportDto.DefaultPackages = packages;
            SetDefaultPackagesCount(packages);

            return this;
        }

        public PackageReportBuilder SetDefaultPackagesUpdates(Func<int, int> predicate)
        {
            _reportDto.DefaultPackages.Values.ToList().ForEach(p =>
            {
                p.ForEach(defPackage =>
                {
                    defPackage.NewTickets = predicate(defPackage.Id);
                });
            });

            return this;
        }

        private void SetDefaultPackagesCount(Dictionary<string, List<PackageFromReportDTO>> packages)
        {
            var count = 0;

            foreach (var serial in packages.AsParallel())
            {
                count += serial.Value.Count;
            }

            _reportDto.DefaultPackagesCount = count;
        }
    }
}
