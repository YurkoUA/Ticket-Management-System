﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.Infrastructure.Exceptions;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Data.Models;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Web.ViewModels.Account;

namespace TicketManagementSystem.Web.Controllers
{
    public class AccountController : ApplicationController<User>
    {
        private AccountService _service;

        public AccountController()
        {
            _service = AccountService.GetInstance();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var user = _service.FindByLogin(User.Identity.Name);

            if (user == null)
                throw new UserNotFoundException();

            return View(new AccountIndexModel
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Role = user.Role.ToString()
            });
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
        public ActionResult Login(LoginModel model)
        {
            if (model == null)
                throw new ModelIsNullException();

            if (!ModelState.IsValid)
                return View(model);

            var user = _service.FindByPassword(model.Login, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Невірний логін або пароль.");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(user.Login, model.Remember);            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}