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

                #region Package

                cfg.CreateMap<PackageDTO, PackageIndexModel>();
                cfg.CreateMap<PackageDTO, PackageDetailsModel>();

                cfg.CreateMap<PackageDTO, PackageMakeDefaultModel>();
                cfg.CreateMap<PackageDTO, PackageMakeSpecialDTO>();
                cfg.CreateMap<PackageMakeDefaultModel, PackageMakeDefaultDTO>();

                cfg.CreateMap<PackageCreateDefaultModel, PackageCreateDTO>();
                cfg.CreateMap<PackageCreateSpecialModel, PackageSpecialCreateDTO>();

                cfg.CreateMap<PackageEditDefaultModel, PackageEditDTO>();
                cfg.CreateMap<PackageEditSpecialModel, PackageSpecialEditDTO>();

                cfg.CreateMap<PackageEditDTO, PackageEditDefaultModel>();
                cfg.CreateMap<PackageSpecialEditDTO, PackageEditSpecialModel>();

                cfg.CreateMap<PackageFilterModel, PackageFilterDTO>();
                cfg.CreateMap<PackageFilterDTO, PackageFilterModel>();

                #endregion

                #region Ticket

                cfg.CreateMap<TicketDTO, TicketDetailsModel>();

                cfg.CreateMap<TicketDTO, TicketChangeNumberModel>();
                cfg.CreateMap<TicketDTO, TicketMoveModel>();
                cfg.CreateMap<TicketDTO, TicketUnallocatedMoveModel>()
                    .ForMember(dest => dest.SerialFull, opt => opt.MapFrom(src => src.SerialName + src.SerialNumber));

                cfg.CreateMap<TicketCreateModel, TicketCreateDTO>();
                cfg.CreateMap<TicketEditDTO, TicketEditModel>();
                cfg.CreateMap<TicketEditModel, TicketEditDTO>();

                #endregion
            });

            return config.CreateMapper();
        }
    }
}