using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace TicketManagementSystem.Web.Areas.Todo.Models
{
    public static class Extensions
    {
        public static ModelStateDictionary ToModelState(IEnumerable<string> array, ModelStateDictionary modelState = null)
        {
            if (modelState == null)
                modelState = new ModelStateDictionary();

            if (!array.Any())
                return modelState;

            array.ToList().ForEach(e =>
            {
                modelState.AddModelError("", e);
            });
            return modelState;
        }

        public static IEnumerable<string> ToEnumerableString(this ModelStateDictionary modelState)
        {
            return modelState.Values.Where(e => e.Errors.Count > 0)
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage);
        }
    }
}