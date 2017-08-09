using System.Web.Http;
using AutoMapper;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public IMapper MapperInstance => AutoMapperConfig.CreateMapper();
    }
}