using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace TicketManagementSystem.Data.EF.Models
{
    public class User : IUser<int>
    {
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int Id { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual IList<Login> Logins { get; set; }

        #region System.Object methods

        public override string ToString() => $"{Email} | {UserName}";

        public override int GetHashCode()
        {
            return Email.GetHashCode() ^ UserName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is User user)
            {
                return user.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)
                    && user.UserName.Equals(UserName);
            }
            return false;
        }

        #endregion
    }
}
