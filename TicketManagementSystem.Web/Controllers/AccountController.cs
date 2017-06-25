using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Business.DTO;
using System.Linq;
using System.Web.UI;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize]
    public class AccountController : ApplicationController
    {
        private IUserService _userService;
        private ILoginService _loginService;

        public AccountController(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User.Identity.GetUserId<int>());

            if (user == null)
                return HttpNotFound();

            return View(MapperInstance.Map<AccountIndexModel>(user));
        }

        [HttpGet, OnlyAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Server)]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, OnlyAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindByLoginDataAsync(model.Login, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль.");
                }
                else
                {
                    var claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer));
                    claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
                    claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role, ClaimValueTypes.String));

                    claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                        "OWIN Provider", ClaimValueTypes.String));

                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.Remember
                    }, claim);

                    var loginDTO = new LoginDTO
                    {
                        UserId = user.Id,
                        Date = DateTime.Now,
                        IpAddress = Request.UserHostAddress,
                        Browser = Request.Browser.Browser,
                        UserAgent = Request.UserAgent
                    };
                    _loginService.AddLogin(loginDTO);

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult LoginHistory()
        {
            var logins = _loginService.GetLoginHistory(User.Identity.GetUserId<int>());

            if (!logins.Any())
                return HttpNotFound();

            return PartialView("LoginHistoryModal", logins);
        }
    }
}