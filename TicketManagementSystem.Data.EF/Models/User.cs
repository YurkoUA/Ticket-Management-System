using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Data.EF.Models
{
    public class User
    {
        public byte[] RowVersion { get; set; }

        public int Id { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string Login { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        public byte[] Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;

        #region System.Object methods

        public override string ToString() => $"{Email} | {Login}";

        public override int GetHashCode()
        {
            return Email.GetHashCode() ^ Login.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (user != null)
            {
                return user.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase)
                    && user.Login.Equals(Login);
            }
            return false;
        }

        #endregion
    }
}
