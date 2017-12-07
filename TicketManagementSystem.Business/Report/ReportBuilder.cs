using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagementSystem.Business.DTO.Report;

namespace TicketManagementSystem.Business.Report
{
    public abstract class ReportBuilder<TReport, TBuilder> where TReport : BaseReportDTO where TBuilder : ReportBuilder<TReport, TBuilder>
    {
        protected TReport _reportDto;

        public TReport Build() => _reportDto;

        public TBuilder SetLastReportDate(DateTime? date)
        {
            _reportDto.LastReportDate = date;
            return (TBuilder)this;
        }

        public TBuilder SetTicketsCount(int count)
        {
            _reportDto.TicketsCount = count;
            return (TBuilder)this;
        }

        public TBuilder SetHappyTicketsCount(int count)
        {
            _reportDto.HappyTicketsCount = count;
            return (TBuilder)this;
        }

        public TBuilder SetDefaultPackagesCount(int count)
        {
            _reportDto.DefaultPackagesCount = count;
            return (TBuilder)this;
        }

        public TBuilder SetSpecialPackages(List<PackageFromReportDTO> packages)
        {
            _reportDto.SpecialPackages = packages;
            _reportDto.SpecialPackagesCount = packages.Count;
            return (TBuilder)this;
        }

        public TBuilder SetNewTicketsCount(int count)
        {
            _reportDto.NewTicketsCount = count;
            return (TBuilder)this;
        }

        public TBuilder SetNewHappyTicketsCount(int count)
        {
            _reportDto.NewHappyTicketsCount = count;
            return (TBuilder)this;
        }

        public TBuilder SetNewPackagesCount(int count)
        {
            _reportDto.NewPackagesCount = count;
            return (TBuilder)this;
        }

        public TBuilder SetSpecialPackagesUpdates(Func<int, int> predicate)
        {
            //_reportDto.SpecialPackages.ForEach(p =>
            //{
            //    p.NewTickets = predicate(p.Id);
            //});

            Parallel.ForEach(_reportDto.SpecialPackages, package =>
            {
                package.NewTickets = predicate(package.Id);
            });

            return (TBuilder)this;
        }
    }
}
