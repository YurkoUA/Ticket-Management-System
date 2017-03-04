﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Data.Interfaces;

namespace TicketManagementSystem.Data.Models
{
    public class Color : IRowVersion
    {
        public byte[] RowVersion { get; set; }

        public int Id { get; set; }

        [StringLength(32, MinimumLength = 3)]
        public string Name { get; set; }

        #region Navigation properties

        public virtual IList<Package> Packages { get; set; } = new List<Package>();
        public virtual IList<Ticket> Tickets { get; set; } = new List<Ticket>();

        #endregion

        #region System.Object methods

        public override string ToString() => Name;

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj)
        {
            var color = obj as Color;

            if (color != null)
            {
                return color.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        #endregion
    }
}