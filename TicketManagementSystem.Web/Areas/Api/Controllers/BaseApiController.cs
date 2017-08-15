using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public IMapper MapperInstance => AutoMapperConfig.CreateMapper();

        public IHttpActionResult NoContent() => StatusCode(HttpStatusCode.NoContent);

        public IHttpActionResult OkOrNoContent<T>(IEnumerable<T> collection)
        {
            if (collection == null || collection.Any())
                return Ok(collection);

            return NoContent();
        }

        public IHttpActionResult OkOrNotFound<T>(T obj)
        {
            if (obj == null)
                return NotFound();

            return Ok(obj);
        }
    }
}