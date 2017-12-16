using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Validation
{
    public class TodoValidationService : ValidationService, ITodoValidationService
    {
        private readonly ITodoService _todoService;

        public TodoValidationService(IUnitOfWork database, ITodoService todoService) : base(database)
        {
            _todoService = todoService;
        }

        public IEnumerable<string> ValidateCreate(TodoTaskDTO dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(dto));

            if (_todoService.ExistsByTitle(dto.Title))
            {
                errors.Add($"Задача \"{dto.Title}\" вже існує.");
            }
            return errors;
        }

        public IEnumerable<string> ValidateEdit(TodoTaskDTO dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(dto));

            if (!_todoService.IsTitleFree(dto.Id, dto.Title))
            {
                errors.Add($"Задача \"{dto.Title}\" вже існує.");
            }
            return errors;
        }
    }
}
