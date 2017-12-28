using System;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }

        #region Dynamic properties for api.

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

        #endregion

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

        public bool IsHappy => Number.IsHappy();
        public int? Clones { get; set; }

        [JsonIgnore]
        public int FirstNumber
        {
            get => int.Parse(Number.First().ToString());
        }

        public override string ToString() => Number;

        public static Expression<Func<Ticket, TicketDTO>> CreateFromTicket = t => new TicketDTO
        {
            Id = t.Id,
            Number = t.Number,

            PackageId = t.PackageId,
            PackageName = t.Package.Name,

            ColorId = t.ColorId,
            ColorName = t.Color.Name,

            SerialId = t.SerialId,
            SerialName = t.Serial.Name,
            SerialNumber = t.SerialNumber,

            Note = t.Note,
            Date = t.Date,
            AddDate = t.AddDate
        };
    }
}
