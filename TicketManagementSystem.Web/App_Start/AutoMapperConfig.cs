using AutoMapper;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Domain.Package.Commands;
using TicketManagementSystem.ViewModels.Color;

namespace TicketManagementSystem.Web
{
    public class AutoMapperConfig
    {
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region Color

                cfg.CreateMap<ColorDTO, ColorIndexModel>();
                cfg.CreateMap<ColorDTO, ColorDetailsModel>();
                cfg.CreateMap<ColorCreateModel, ColorCreateDTO>();

                cfg.CreateMap<ColorEditModel, ColorEditDTO>();
                cfg.CreateMap<ColorEditDTO, ColorEditModel>();

                cfg.CreateMap<ColorVM, ColorDetailsModel>();

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

                cfg.CreateMap<PackageDTO, PackageDetailsModel>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ToString()));

                cfg.CreateMap<PackageDTO, PackageMakeSpecialDTO>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ToString()));

                cfg.CreateMap<PackageDTO, PackageMakeDefaultModel>();

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

                cfg.CreateMap<TicketDTO, TicketMoveModel>()
                    .ForMember(dest => dest.IsUnallocated, opt => opt.MapFrom(src => src.PackageId == null));

                cfg.CreateMap<TicketDTO, TicketUnallocatedMoveModel>()
                    .ForMember(dest => dest.SerialFull, opt => opt.MapFrom(src => src.SerialName + src.SerialNumber));

                cfg.CreateMap<TicketCreateModel, TicketCreateDTO>();
                cfg.CreateMap<TicketEditDTO, TicketEditModel>();
                cfg.CreateMap<TicketEditModel, TicketEditDTO>();

                #endregion

                #region CQRS

                cfg.CreateMap<PackageCreateDefaultModel, CreatePackageCommand>()
                    .ForMember(dest => dest.FirstDigit, opt => opt.MapFrom(src => src.FirstNumber));

                cfg.CreateMap<PackageCreateSpecialModel, CreateSpecialPackageCommand>();

                cfg.CreateMap<PackageEditDefaultModel, EditPackageCommand>()
                    .ForMember(dest => dest.FirstDigit, opt => opt.MapFrom(src => src.FirstNumber));

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