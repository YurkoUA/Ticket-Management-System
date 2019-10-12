using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Data.EF.Models
{
    [Table("Report")]
    public class Report
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
        public bool IsAutomatic { get; set; }

        #region System.Object methods.

        public override string ToString()
        {
            return $"Report_{Id}";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Report report)
            {
                return report.Id == Id;
            }
            return false;
        }

        #endregion
    }
}
