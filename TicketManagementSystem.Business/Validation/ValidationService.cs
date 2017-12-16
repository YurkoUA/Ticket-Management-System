using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TicketManagementSystem.Data.EF.Interfaces;

namespace TicketManagementSystem.Business.Validation
{
    public class ValidationService : Service
    {
        public ValidationService(IUnitOfWork database) : base(database)
        {
        }

        protected IEnumerable<string> ValidateObject<T>(T obj)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
            return results.Select(e => e.ErrorMessage);
        }
    }
}
