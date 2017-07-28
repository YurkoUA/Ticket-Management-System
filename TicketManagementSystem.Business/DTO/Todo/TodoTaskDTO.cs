using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TicketManagementSystem.Data.EF.Enums;

namespace TicketManagementSystem.Business.DTO
{
    public class TodoTaskDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();

        [Required(ErrorMessage = "Необхідно вказати назву.")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Назва задачі повинна бути від 3 до 16 символів.")]
        public string Title { get; set; }

        [StringLength(128, ErrorMessage = "Опис не може бути більшим за 128 символів.")]
        public string Description { get; set; }

        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.None;

        public string PriorityString => (int)Priority != 0 ? GetEnumDisplayName(Priority) : "";
        public string StatusString => GetEnumDisplayName(Status);

        private string GetEnumDisplayName(Enum member)
        {
            return member.GetType()
                .GetMember(member.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;
        }
    }
}
