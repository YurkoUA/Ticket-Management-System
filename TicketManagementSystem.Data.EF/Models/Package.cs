﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Data.EF.Models
{
    public class Package : IRowVersion
    {
        public byte[] RowVersion { get; set; }

        public int Id { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        public int? ColorId { get; set; }
        public int? SerialId { get; set; }
        public int? FirstNumber { get; set; }
        public double Nominal { get; set; }
        public bool IsSpecial { get; set; } = false;
        public bool IsOpened { get; set; } = true;
        public DateTime Date { get; set; } = DateTime.Now;
        
        [StringLength(128)]
        public string Note { get; set; }

        #region Navigation properties

        public Color Color { get; set; }
        public Serial Serial { get; set; }
        public IList<Ticket> Tickets { get; set; }

        #endregion

        #region System.Object methods

        public override string ToString()
        {
            if (IsSpecial)
                return Name;

            var toString = new StringBuilder();

            if (SerialId != null)
            {
                toString.Append(Serial.ToString());

                if (ColorId != null)
                    toString.Append("-");
            }

            if (ColorId != null)
            {
                toString.Append(Color.ToString());
            }

            if (Tickets.Count > 0)
            {
                toString.Append($" ({Tickets.First().ToString()})");
            }

            return toString.ToString();
        }

        public override int GetHashCode()
        {
            if (IsSpecial)
                return Name.GetHashCode();

            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var package = obj as Package;

            if (package != null)
            {
                if (IsSpecial)
                    return package.Name.Equals(Name);
            }
            return false;
        }

        #endregion
    }
}
