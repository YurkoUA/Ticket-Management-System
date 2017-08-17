using System;
using System.Linq;
using Newtonsoft.Json;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public dynamic Package
        {
            get
            {
                if (PackageId == null)
                    return null;

                return new { Id = PackageId, Name = PackageName };
            }
        }

        public dynamic Color => new { Id = ColorId, Name = ColorName };
        public dynamic Serial => new { Id = SerialId, Name = SerialName };

        [JsonIgnore]
        public int? PackageId { get; set; }
        [JsonIgnore]
        public string PackageName { get; set; }

        [JsonIgnore]
        public int ColorId { get; set; }
        [JsonIgnore]
        public string ColorName { get; set; }

        [JsonIgnore]
        public int SerialId { get; set; }
        [JsonIgnore]
        public string SerialName { get; set; }

        public string SerialNumber { get; set; }

        public string Note { get; set; }

        public string Date { get; set; }
        public DateTime AddDate { get; set; }

        public bool IsHappy { get; set; }

        [JsonIgnore]
        public int FirstNumber
        {
            get => int.Parse(Number.First().ToString());
        }
    }
}
