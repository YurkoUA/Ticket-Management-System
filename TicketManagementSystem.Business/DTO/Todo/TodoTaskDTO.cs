using System;
using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Data.EF.Enums;

namespace TicketManagementSystem.Business.DTO
{
    public class TodoTaskDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();

        [Required(ErrorMessage = "Необхідно вказати назву.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Назва задачі повинна бути від 3 до 64 символи.")]
        public string Title { get; set; }

        [StringLength(256, ErrorMessage = "Опис не може бути більшим за 256 символів.")]
        public string Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.VeryLow;
        public TaskStatus Status { get; set; } = TaskStatus.None;

        public string PriorityString => Priority.GetDisplayName();
        public string StatusString => Status.GetDisplayName();

        public override string ToString() => Title;
    }
}
