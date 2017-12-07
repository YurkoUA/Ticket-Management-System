using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Business.AppSettings;

namespace TicketManagementSystem.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IAppSettingsService _appSettingsService;

        public AccountController(IUserService userService, ILoginService loginService, IAppSettingsService appSettingsService)
        {
            _userService = userService;
            _loginService = loginService;
            _appSettingsService = appSettingsService;
        }

        [HttpGet, Authorize, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User.Identity.GetUserId<int>());

            if (user == null)
                return HttpNotFound();

            return View(MapperInstance.Map<AccountIndexModel>(user));
        }

        [HttpGet, OnlyAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public ActionResult Login() => View();

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
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.Remember
                    }, GetClaimsIdentity(user));

                    _loginService.AddLogin(GetLoginDto(Request, user), _appSettingsService.RemoveOldLogins);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet, Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, Authorize, OutputCache(Duration = 20, Location = OutputCacheLocation.Client)]
        public ActionResult LoginHistory()
        {
            var logins = _loginService.GetLoginHistory(User.Identity.GetUserId<int>(), _appSettingsService.LatestLoginsToShow);

            if (!logins.Any())
                return HttpNotFound();

            return PartialView("LoginHistoryModal", logins);
        }

        [HttpGet, Authorize, OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult ChangePassword() => PartialView("ChangePasswordModal");

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindByLoginDataAsync(User.Identity.GetUserName(), viewModel.CurrentPassword);

                if (user == null)
                    ModelState.AddModelError("", "Поточний пароль введено невірно.");

                if (ModelState.IsValid)
                {
                    await _userService.ChangePasswordAsync(user.Id, viewModel.NewPassword);
                    return SuccessPartial("Пароль успішно змінено.");
                }
            }
            return ErrorPartial(ModelState);
        }

        private LoginDTO GetLoginDto(HttpRequestBase request, UserDTO user)
        {
            return new LoginDTO
            {
                UserId = user.Id,
                Date = DateTime.UtcNow,
                IpAddress = Request.UserHostAddress,
                Browser = Request.Browser.Browser,
                UserAgent = Request.UserAgent,
                Type = "ApplicationCookie",
                Host = $"{request.Url.Scheme}://{request.Url.Authority}"
            };
        }

        private static ClaimsIdentity GetClaimsIdentity(UserDTO user)
        {
            var claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer));
            claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role, ClaimValueTypes.String));

            claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "OWIN Provider", ClaimValueTypes.String));

            return claim;
        }
    }
}