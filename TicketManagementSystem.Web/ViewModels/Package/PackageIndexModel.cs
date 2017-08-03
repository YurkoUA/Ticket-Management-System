using System.Collections;
using System.Collections.Generic;
using TicketManagementSystem.Business.Enums;

namespace TicketManagementSystem.Web
{
    public class PackageIndexModel : IEnumerable<PackageDetailsModel>
    {
        public IEnumerable<PackageDetailsModel> Packages { get; set; }
        public PageInfo PageInfo { get; set; }
        public PackagesFilter Filter { get; set; }

        public int TotalPackages { get; set; }
        public int OpenedPackages { get; set; }
        public int SpecialPackages { get; set; }

        public string ClassAll => Filter == PackagesFilter.All ? "active" : "";

        public string ClassOpened
        {
            get
            {
                if (OpenedPackages == 0)
                    return "disabled";

                return Filter == PackagesFilter.Opened ? "active" : "";
            }
        }

        public string ClassSpecial
        {
            get
            {
                if (SpecialPackages == 0)
                    return "disabled";

                return Filter == PackagesFilter.Special ? "active" : "";
            }
        }

        public IEnumerator<PackageDetailsModel> GetEnumerator()
        {
            return Packages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Packages.GetEnumerator();
        }
    }
}