using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Data.EF.Models;
using TicketManagementSystem.Web.Filters;

namespace TicketManagementSystem.Web.Controllers
{
    public class AccountController : ApplicationController
    {
        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User.Identity.GetUserId<int>());

            if (user == null)
                return HttpNotFound();

            return View(MapperInstance.Map<AccountIndexModel>(user));
        }

        [HttpGet]
        [OnlyAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [OnlyAnonymous]
        [ValidateAntiForgeryToken]
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

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public async Task<ActionResult> Register(string email, string userName, string password)
        //{
        //    await _userService.CreateAsync(new User { Email = email, UserName = userName }, password);
        //    return null;
        //}
    }
}