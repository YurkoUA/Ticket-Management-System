using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Data.EF.Enums;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ITodoService
    {
        IEnumerable<TodoTaskDTO> GetTasks();
        IEnumerable<TodoTaskDTO> GetTasks(TaskStatus status);
        Dictionary<TaskStatus, IEnumerable<TodoTaskDTO>> GetTasksGroupByStatus();
        Dictionary<TaskStatus, IEnumerable<TodoTaskDTO>> GetTasksGroupByStatus(int take);

        TodoTaskDTO GetById(int id);

        TodoTaskDTO Create(TodoTaskDTO taskDTO);
        void Update(TodoTaskDTO taskDTO);
        void Delete(int id);

        void SetStatus(int id, TaskStatus status);
        void SetPriority(int id, TaskPriority priority);

        bool ExistsByTitle(string title);
        bool IsTitleFree(int id, string ntitle);

        IEnumerable<string> ValidateCreate(TodoTaskDTO dto);
        IEnumerable<string> ValidateEdit(TodoTaskDTO dto);
    }
}
