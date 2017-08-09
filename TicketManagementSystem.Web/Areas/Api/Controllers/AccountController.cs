using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TicketManagementSystem.Business.Extensions;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [Authorize]
    public class AccountController : BaseApiController
    {
        private IUserService _userService;
        private ILoginService _loginService;

        public AccountController(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var user = (await _userService.FindByIdAsync(User.Identity.GetUserId<int>())).ToDto();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public IHttpActionResult LoginHistory(int take)
        {
            if (take < 1)
                take = 10;

            var logins = _loginService.GetLoginHistory(User.Identity.GetUserId<int>(), take);

            if (!logins.Any())
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(logins);
        }
    }
}
