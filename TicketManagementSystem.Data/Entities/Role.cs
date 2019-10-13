using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace TicketManagementSystem.Data.Entities
{
    [Table("Role")]
    public class Role : IRole<int>
    {
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(32, MinimumLength = 4)]
        public string Description { get; set; }

        #region Navigation properties

        public IList<User> Users { get; set; } = new List<User>();

        #endregion

        #region System.Object methods

        public override string ToString()
        {
            return Description;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Description.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Role role)
            {
                return role.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase)
                    && role.Description.Equals(Description, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        #endregion
    }
}
