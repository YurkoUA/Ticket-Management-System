using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using AutoMapper;
using TicketManagementSystem.Domain.DTO;

namespace TicketManagementSystem.Bootstrap.Mapping.Resolvers
{
    public class CommandMessageDTOResolver : ITypeConverter<IEnumerable<CommandMessageDTO>, IEnumerable<string>>
    {
        public IEnumerable<string> Convert(IEnumerable<CommandMessageDTO> source, IEnumerable<string> destination, ResolutionContext context)
        {
            var result = new List<string>();
            var rm = new ResourceManager("ValidationMessage.resx", Assembly.GetExecutingAssembly());

            foreach (var item in source)
            {
                var message = rm.GetString(item.ResourceName);
                message = string.Format(message, item.Arguments);
                result.Add(message);
            }

            return result;
        }
    }
}
