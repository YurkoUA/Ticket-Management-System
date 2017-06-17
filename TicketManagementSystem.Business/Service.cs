using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using TicketManagementSystem.Data.EF.Interfaces;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace TicketManagementSystem.Business
{
    public abstract class Service
    {
        public IUnitOfWork Database { get; set; }
        public IMapper MapperInstance { get; }

        public Service(IUnitOfWork database)
        {
            Database = database;
            MapperInstance = AutoMapperConfig.CreateMapper();
        }

        protected IEnumerable<string> ValidateObject<T>(T @object)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(@object, new ValidationContext(@object), results, true);
            return results.Select(e => e.ErrorMessage);
        }
    }
}
