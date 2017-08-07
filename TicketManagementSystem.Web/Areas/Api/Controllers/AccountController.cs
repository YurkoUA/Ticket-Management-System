using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult TestAuthorize()
        {
            return Ok("Ok");
        }
    }
}
