using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.DTO
{
    public class PackageDTO
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public int TicketsCount { get; set; }

        [JsonIgnore]
        public int? ColorId { get; set; }
        [JsonIgnore]
        public int? SerialId { get; set; }
        [JsonIgnore]
        public int? NominalId { get; set; }

        [JsonIgnore]
        public string ColorName { get; set; }
        [JsonIgnore]
        public string SerialName { get; set; }

        #region Dynamic properties for API.

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

        [JsonProperty("Name")]
        public dynamic NameToString => ToString();

        #endregion

        public int? FirstNumber { get; set; }
        public double? Nominal { get; set; }

        public bool IsSpecial { get; set; }
        public bool IsOpened { get; set; }

        public string Note { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public string FirstTicketNumber { get; set; }

        [JsonIgnore]
        public string SelectListOptionValue
        {
            get
            {
                var retValue = ToString();

                if (TicketsCount > 0)
                    retValue += $": {TicketsCount} шт.";

                return retValue;
            }
        }

        public override string ToString()
        {
            if (IsSpecial)
                return Name;

            var toString = new StringBuilder();

            if (SerialId != null)
            {
                toString.Append(SerialName);

                if (ColorId != null)
                    toString.Append("-");
            }

            if (ColorId != null)
            {
                toString.Append(ColorName);
            }

            if (TicketsCount > 0)
            {
                toString.Append($" ({FirstTicketNumber})");
            }

            return toString.ToString();
        }

        public static Expression<Func<Package, PackageDTO>> CreateFromPackage = p => new PackageDTO
        {
            Id = p.Id,
            Name = p.Name,
            TicketsCount = p.Tickets.Count,

            ColorId = p.ColorId,
            ColorName = p.Color.Name,

            SerialId = p.SerialId,
            SerialName = p.Serial.Name,

            NominalId = p.NominalId,
            
            FirstNumber = p.FirstNumber,
            Nominal = p.NominalEntity.Amount,

            IsSpecial = p.IsSpecial,
            IsOpened = p.IsOpened,

            Note = p.Note,
            Date = p.Date,

            FirstTicketNumber = p.Tickets.OrderBy(t => t.Id).FirstOrDefault().Number
        };
    }
}
