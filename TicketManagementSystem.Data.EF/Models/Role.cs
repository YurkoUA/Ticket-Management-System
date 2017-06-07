using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TicketManagementSystem.Data.EF.Models
{
    public class Role : IRole<int>
    {
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 4)]
        public string Name { get; set; }

        // TODO: Add role description property.

        #region Navigation properties

        public virtual IList<User> Users { get; set; } = new List<User>();

        #endregion

        #region System.Object methods

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Role role)
            {
                return role.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        #endregion
    }
}
