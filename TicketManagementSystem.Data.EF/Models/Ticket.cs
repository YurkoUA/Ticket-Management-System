using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TicketManagementSystem.Data.EF.Models
{
    public class Ticket
    {
        public byte[] RowVersion { get; set; }

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
        public DateTime AddDate { get; set; } = DateTime.Now;

        #region Navigation properties

        public virtual Package Package { get; set; }
        public virtual Color Color { get; set; }
        public virtual Serial Serial { get; set; }

        #endregion

        public bool IsHappy()
        {
            var numbers = Number.Select(n => int.Parse(n.ToString())).ToArray();
            return numbers[0] + numbers[1] + numbers[2] == numbers[3] + numbers[4] + numbers[5];
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
                return Number.Equals(ticket.Number, StringComparison.CurrentCultureIgnoreCase)
                    && ColorId == ticket.ColorId && SerialId == ticket.SerialId
                    && SerialNumber.Equals(ticket.SerialNumber, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        #endregion
    }
}
