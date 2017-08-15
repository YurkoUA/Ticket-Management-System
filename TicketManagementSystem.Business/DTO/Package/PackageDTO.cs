using System;
using Newtonsoft.Json;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TicketsCount { get; set; }

        [JsonIgnore]
        public int? ColorId { get; set; }
        [JsonIgnore]
        public int? SerialId { get; set; }

        [JsonIgnore]
        public string ColorName { get; set; }
        [JsonIgnore]
        public string SerialName { get; set; }

        public dynamic Color
        {
            get
            {
                if (ColorId == null)
                    return null;

                return new { Id = ColorId, Name = ColorName };
            }
        }

        public dynamic Serial
        {
            get
            {
                if (SerialId == null)
                    return null;

                return new { Id = SerialId, Name = SerialName };
            }
        }
        
        public int? FirstNumber { get; set; }
        public double Nominal { get; set; }

        public bool IsSpecial { get; set; }
        public bool IsOpened { get; set; }

        public string Note { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
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
