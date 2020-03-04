using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagementSystem.Bootstrap.Mapping.Resolvers;
using TicketManagementSystem.Data.Entities;
using TicketManagementSystem.Domain.DTO;
using TicketManagementSystem.Domain.DTO.Ticket;
using TicketManagementSystem.Domain.Package.Commands;
using TicketManagementSystem.Domain.Ticket.Commands;
using TicketManagementSystem.ViewModels.Common;
using TicketManagementSystem.ViewModels.Nominal;

namespace TicketManagementSystem.Bootstrap.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap(typeof(CommandResultDTO<>), typeof(CommandResultVM<>));

            CreateMap<CommandMessageDTO, string>()
                .ConvertUsing<CommandMessageDTOResolver>();

            CreateMap<Nominal, NominalVM>();

            #region Package.

            CreateMap<CreatePackageCommand, Package>()
                .ForMember(dest => dest.FirstNumber, opt => opt.MapFrom(src => src.FirstDigit));

            CreateMap<CreateSpecialPackageCommand, Package>()
                .ForMember(dest => dest.IsSpecial, opt => opt.UseValue(true));

            CreateMap<EditPackageCommand, Package>()
                .ForMember(dest => dest.FirstNumber, opt => opt.MapFrom(src => src.FirstDigit));

            CreateMap<EditSpecialPackageCommand, Package>();

            #endregion

            #region Ticket.

            CreateMap<CreateTicketCommand, TicketDTO>();
            CreateMap<EditTicketCommand, TicketDTO>();
            CreateMap<CreateTicketCommand, Ticket>();

            #endregion
        }
    }
}
