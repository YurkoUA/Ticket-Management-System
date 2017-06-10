﻿using AutoMapper;
using TicketManagementSystem.Data.EF.Models;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMapper()
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
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

                #endregion

                #region Package

                cfg.CreateMap<Package, PackageDTO>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ToString()))
                    .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ToString()))
                    .ForMember(dest => dest.SerialName, opt => opt.MapFrom(src => src.Serial.ToString()))
                    .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count));

                cfg.CreateMap<Package, PackageEditDTO>()
                    .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count));

                cfg.CreateMap<Package, PackageSpecialEditDTO>()
                    .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count));

                cfg.CreateMap<PackageCreateDTO, Package>();
                cfg.CreateMap<PackageSpecialCreateDTO, Package>();

                #endregion
            });

            return config.CreateMapper();
        }
    }
}
