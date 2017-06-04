using AutoMapper;
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
            });

            return config.CreateMapper();
        }
    }
}
