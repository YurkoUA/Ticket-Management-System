using System;
using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Data.EF.Enums;

namespace TicketManagementSystem.Data.EF.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required, StringLength(64, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.VeryLow;
        public TaskStatus Status { get; set; }

        #region System.Object methods

        public override string ToString()
        {
            return Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TodoTask task)
            {
                return task.Title.Equals(Title, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        #endregion
    }
}
