using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Data.EF.Models
{
    public class Serial : IRowVersion
    {
        public byte[] RowVersion { get; set; }

        public int Id { get; set; }

        [StringLength(4, MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Note { get; set; }

        #region Navigation properties

        public IList<Package> Packages { get; set; }
        public IList<Ticket> Tickets { get; set; }

        #endregion

        #region System.Object methods

        public override string ToString() => Name;

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj)
        {
            var serial = obj as Serial;

            if (serial != null)
            {
                return serial.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        #endregion
    }
}
