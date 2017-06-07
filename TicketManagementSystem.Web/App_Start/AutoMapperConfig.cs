using AutoMapper;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Web
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region Color

                cfg.CreateMap<ColorDTO, ColorIndexModel>();
                cfg.CreateMap<ColorDTO, ColorDetailsModel>();
                cfg.CreateMap<ColorCreateModel, ColorCreateDTO>();

                cfg.CreateMap<ColorEditModel, ColorEditDTO>();
                cfg.CreateMap<ColorEditDTO, ColorEditModel>();

                #endregion

                #region Serial

                cfg.CreateMap<SerialDTO, SerialIndexModel>();
                cfg.CreateMap<SerialDTO, SerialDetailsModel>();
                cfg.CreateMap<SerialCreateModel, SerialCreateDTO>();

                cfg.CreateMap<SerialEditModel, SerialEditDTO>();
                cfg.CreateMap<SerialEditDTO, SerialEditModel>();

                #endregion

                #region User

                cfg.CreateMap<UserDTO, AccountIndexModel>();

                #endregion
            });

            return config.CreateMapper();
        }
    }
}