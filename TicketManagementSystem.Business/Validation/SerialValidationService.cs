using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Validation
{
    public class SerialValidationService : ValidationService, ISerialValidationService
    {
        private readonly ISerialService _serialService;

        public SerialValidationService(IUnitOfWork database, ISerialService serialService) : base(database)
        {
            _serialService = serialService;
        }

        public IEnumerable<string> Validate(SerialCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (_serialService.ExistsByName(createDTO.Name))
            {
                errors.Add($"Серія \"{createDTO.Name}\" вже існує.");
            }
            return errors;
        }

        public IEnumerable<string> Validate(SerialEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));

            if (!_serialService.IsNameFree(editDTO.Id, editDTO.Name))
            {
                errors.Add($"Серія \"{editDTO.Name}\" вже існує.");
            }
            return errors;
        }
    }
}
