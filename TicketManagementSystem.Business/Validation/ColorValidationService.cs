using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Validation
{
    public class ColorValidationService : ValidationService, IColorValidationService
    {
        private readonly IColorService _colorService;

        public ColorValidationService(IUnitOfWork database, IColorService colorService) : base(database)
        {
            _colorService = colorService;
        }

        public IEnumerable<string> Validate(ColorCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (_colorService.ExistsByName(createDTO.Name))
            {
                errors.Add($"Колір \"{createDTO.Name}\" вже існує.");
            }

            return errors;
        }

        public IEnumerable<string> Validate(ColorEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));

            if (!_colorService.IsNameFree(editDTO.Id, editDTO.Name))
            {
                errors.Add($"Колір \"{editDTO.Name}\" вже існує.");
            }

            return errors;
        }
    }
}
