using System.Collections;
using System.Collections.Generic;

namespace TicketManagementSystem.Web
{
    public class PackageIndexModel : IEnumerable<PackageDetailsModel>
    {
        public IEnumerable<PackageDetailsModel> Packages { get; set; }
        public PageInfo PageInfo { get; set; }

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