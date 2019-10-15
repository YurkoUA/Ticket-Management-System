﻿using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using AutoMapper;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.Resources;

namespace TicketManagementSystem.Bootstrap.Mapping.Resolvers
{
    public class CommandMessageDTOResolver : ITypeConverter<CommandMessageDTO, string>
    {
        public string Convert(CommandMessageDTO source, string destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            var rm = new ResourceManager("TicketManagementSystem.Domain.Resources.ValidationMessage", typeof(ValidationMessage).Assembly);

            var message = rm.GetString(source.ResourceName);
            message = string.Format(message, source.Arguments);
            return message;
        }
    }
}