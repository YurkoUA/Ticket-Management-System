using System.Linq;
using AutoMapper;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business
{
    public class AutoMapperConfig
    {
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region Color

                cfg.CreateMap<Color, ColorDTO>()
                    .ForMember(dest => dest.PackagesCount, opt => opt.MapFrom(src => src.Packages.Count))
                    .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count)
                );

                cfg.CreateMap<Color, ColorEditDTO>()
                    .ForMember(dest => dest.CanBeDeleted, 
                        opt => opt.MapFrom(src => src.Packages.Count == 0 && src.Tickets.Count == 0)
                );

                cfg.CreateMap<ColorCreateDTO, Color>();

                #endregion

                #region Serial

                cfg.CreateMap<Serial, SerialDTO>()
                     .ForMember(dest => dest.PackagesCount, opt => opt.MapFrom(src => src.Packages.Count))
                     .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count)
                );

                cfg.CreateMap<Serial, SerialEditDTO>()
                    .ForMember(dest => dest.CanBeDeleted, opt => opt.MapFrom(src => src.Packages.Count == 0 && src.Tickets.Count == 0)
                );

                cfg.CreateMap<SerialCreateDTO, Serial>();

                #endregion

                #region User

                cfg.CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
                    .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => src.Role.ToString()));

                #endregion

                #region Login

                cfg.CreateMap<Login, LoginDTO>();
                cfg.CreateMap<LoginDTO, Login>();

                #endregion

                #region Report

                cfg.CreateMap<Data.Entities.Report, DTO.Report.ReportDTO>();
                cfg.CreateMap<DTO.Report.ReportDTO, Data.Entities.Report>();

                #endregion

                #region Package

                cfg.CreateMap<Package, PackageDTO>()
                    .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ToString()))
                    .ForMember(dest => dest.SerialName, opt => opt.MapFrom(src => src.Serial.ToString()))
                    .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count))
                    .ForMember(dest => dest.FirstTicketNumber, opt => opt.MapFrom(src => src.Tickets.OrderBy(t => t.Id).FirstOrDefault().Number));

                cfg.CreateMap<Package, PackageEditDTO>()
                    .ForMember(dest => dest.IsEmpty, opt => opt.MapFrom(src => !src.Tickets.Any()));

                cfg.CreateMap<PackageCreateDTO, Package>();
                cfg.CreateMap<PackageSpecialCreateDTO, Package>();

                #endregion

                #region Ticket

                cfg.CreateMap<Ticket, TicketDTO>()
                    .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ToString()))
                    .ForMember(dest => dest.SerialName, opt => opt.MapFrom(src => src.Serial.ToString()))
                    .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.ToString()));

                cfg.CreateMap<TicketDTO, Ticket>()
                    .ForMember(dest => dest.Package, opt => opt.Ignore())
                    .ForMember(dest => dest.Color, opt => opt.Ignore())
                    .ForMember(dest => dest.Serial, opt => opt.Ignore());

                cfg.CreateMap<TicketCreateDTO, Ticket>();
                cfg.CreateMap<TicketCreateDTO, TicketDTO>();
                cfg.CreateMap<TicketEditDTO, TicketDTO>();
                cfg.CreateMap<Ticket, TicketEditDTO>();

                #endregion

                #region Summary

                cfg.CreateMap<Summary, SummaryDTO>();

                #endregion
            });

            return config.CreateMapper();
        }

        private static IMapper mapper;

        public static IMapper GetInstance()
        {
            if (mapper == null)
                mapper = CreateMapper();

            return mapper;
        }
    }
}
