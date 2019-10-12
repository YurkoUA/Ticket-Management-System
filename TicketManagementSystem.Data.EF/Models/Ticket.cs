using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TicketManagementSystem.Data.EF.Models
{
    [Table("Ticket")]
    public class Ticket : IComparable<Ticket>
    {
        public int Id { get; set; }

        [StringLength(6, MinimumLength = 6)]
        public string Number { get; set; }

        public int? PackageId { get; set; }
        public int ColorId { get; set; }
        public int SerialId { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string SerialNumber { get; set; }
        
        [StringLength(128)]
        public string Note { get; set; }

        [StringLength(32)]
        public string Date { get; set; }
        public DateTime AddDate { get; set; } = DateTime.UtcNow;

        #region Navigation properties

        public Package Package { get; set; }
        public Color Color { get; set; }
        public Serial Serial { get; set; }

        #endregion

        public int CompareTo(Ticket other)
        {
            return int.Parse(Number.First().ToString())
                .CompareTo(int.Parse(other.Number.First().ToString()));
        }

        #region System.Object methods

        public override string ToString() => Number;

        public override int GetHashCode()
        {
            return Number.GetHashCode() ^ ColorId.GetHashCode()
                ^ SerialId.GetHashCode() ^ SerialNumber.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Ticket ticket)
            {
                return Number.Equals(ticket.Number)
                    && ColorId == ticket.ColorId && SerialId == ticket.SerialId
                    && SerialNumber.Equals(ticket.SerialNumber);
            }
            return false;
        }

        #endregion
    }
}
