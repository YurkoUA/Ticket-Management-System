using System;
using System.Linq;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketDTO
    {
        public byte[] RowVersion { get; set; }
        
        public int Id { get; set; }
        public string Number { get; set; }

        public int? PackageId { get; set; }
        public string PackageName { get; set; }

        public int ColorId { get; set; }
        public string ColorName { get; set; }

        public int SerialId { get; set; }
        public string SerialName { get; set; }

        public string SerialNumber { get; set; }

        public string Note { get; set; }

        public string Date { get; set; }
        public DateTime AddDate { get; set; }

        public bool IsHappy { get; set; }

        public int FirstNumber
        {
            get => int.Parse(Number.First().ToString());
        }
    }
}
