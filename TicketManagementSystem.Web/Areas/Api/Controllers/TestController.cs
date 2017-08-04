using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult TestMethod(int a, int b)
        {
            return Ok(a + b);
        }
    }
}
