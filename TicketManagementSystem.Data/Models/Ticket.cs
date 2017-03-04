﻿using System;
using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Data.Models
{
    public class Ticket : IRowVersion
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

        public DateTime? Date { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;

        #region Navigation properties

        public virtual Package Package { get; set; }
        public virtual Color Color { get; set; }
        public virtual Serial Serial { get; set; }

        #endregion

        #region System.Object methods

        public override string ToString() => Number;

        public override int GetHashCode()
        {
            return Number.GetHashCode() ^ ColorId.GetHashCode()
                ^ SerialId.GetHashCode() ^ SerialNumber.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var ticket = obj as Ticket;

            if (ticket != null)
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