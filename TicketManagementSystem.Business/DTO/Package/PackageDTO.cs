using System;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TicketsCount { get; set; }

        public int? ColorId { get; set; }
        public int? SerialId { get; set; }

        public string ColorName { get; set; }
        public string SerialName { get; set; }

        public int? FirstNumber { get; set; }
        public double Nominal { get; set; }

        public bool IsSpecial { get; set; }
        public bool IsOpened { get; set; }

        public string Note { get; set; }
        public DateTime Date { get; set; }

        public string SelectListOptionValue
        {
            get
            {
                var retValue = Name;

                if (TicketsCount > 0)
                    retValue += $": {TicketsCount} шт.";

                return retValue;
            }
        }
    }
}
