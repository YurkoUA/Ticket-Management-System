using System;
using TicketManagementSystem.Business.Enums;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageCountDTO
    {
        public int Total { get; set; }
        public int Opened { get; set; }
        public int Special { get; set; }

        public int Where(PackagesFilter filter)
        {
            switch (filter)
            {
                case PackagesFilter.All:
                    return Total;

                case PackagesFilter.Opened:
                    return Opened;

                case PackagesFilter.Special:
                    return Special;

                default:
                    throw new ArgumentNullException();
            }
        }
    }
}
