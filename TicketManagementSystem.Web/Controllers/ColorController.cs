using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagementSystem.Business.Infrastructure.Exceptions;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Data.Models;
using TicketManagementSystem.Enumerations;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Web.ViewModels.Color;

namespace TicketManagementSystem.Web.Controllers
{
    public class ColorController : ApplicationController<Color>
    {
        private ColorService _service;

        public ColorController()
        {
            _service = ColorService.GetInstance();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var colours = _service.Repository.GetAll();

            return View(colours.Select(m => new ColorIndexModel
            {
                Id = m.Id,
                Name = m.Name,
                PackagesCount = m.Packages.Count,
                TicketsCount = m.Tickets.Count
            }));
        }

        [HttpGet]
        public ActionResult Details(int? id, bool partial = false)
        {
            if (id == null)
                return RedirectToAction("Index");

            var color = _service.Repository.GetById((int)id);

            if (color == null)
                return NotFound();

            var viewModel = new ColorDetailsModel
            {
                Id = color.Id,
                Name = color.Name,
                PackagesCount = color.Packages.Count,
                TicketsCount = color.Tickets.Count
            };

            if (partial)
                return PartialView("DetailsPartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Колір \"{viewModel.Name}\"";
            return View("Color", ViewMode.Details);
        }

        [HttpGet]
        [Admin]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Admin]
        public ActionResult Create(ColorCreateModel model)
        {
            if (model == null)
                throw new ModelIsNullException();

            if (!ModelState.IsValid)
            {
                return ErrorPartial(ModelState);
            }

            if (_service.Repository.Contains(m => m.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                ModelState.AddModelError("", $"Колір \"{model.Name}\" вже існує.");

                return ErrorPartial(ModelState);
            }

            var id = _service.CreateColor(model.Name, model.RowVersion).Id;

            return SuccessAlert($"Колір \"{model.Name}\" успішно додано!", 
                Url.Action("Details", "Color", new { id = id }), 
                "Переглянути");
        }

        [HttpGet]
        [Admin]
        public ActionResult Edit(int? id, bool partial = false)
        {
            if (id == null)
                return RedirectToAction("Index");

            var color = _service.Repository.GetById((int)id);

            if (color == null)
                return NotFound();

            var viewModel = new ColorEditModel
            {
                Id = color.Id,
                Name = color.Name,
                RowVersion = color.RowVersion,
                CanBeDeleted = !color.Packages.Any() && !color.Tickets.Any()
            };

            if (partial)
                return PartialView("EditPartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Редагування кольору \"{viewModel.Name}\"";
            return View("Color", ViewMode.Edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Admin]
        public ActionResult Edit(ColorEditModel model)
        {
            if (model == null)
                throw new ModelIsNullException();

            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (!_service.CanBeEdited(model.Id, model.Name))
            {
                ModelState.AddModelError("", $"Колір \"{model.Name}\" вже існує!");
                return ErrorPartial(ModelState);
            }

            _service.EditColor(model.Id, model.Name, model.RowVersion);

            return SuccessAlert("Колір успішно відредаговано!");
        }

        [HttpGet]
        [Admin]
        public ActionResult Delete(int? id, bool partial = false)
        {
            if (id == null)
                return RedirectToAction("Index");

            if (!_service.Repository.ExistsById((int)id))
                return NotFound();

            var viewModel = _service.Repository.GetById((int)id);

            if (partial)
                return PartialView("DeletePartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Видалення кольору \"{viewModel.Name}\"";
            return View("Color", ViewMode.Delete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Admin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var color = _service.Repository.GetById((int)id);

            if (color == null)
                return NotFound();

            if (color.Packages.Count == 0 && color.Tickets.Count == 0)
            {
                _service.RemoveColor((int)id);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до нього належать!");
            return ErrorPartial(ModelState);
        }
    }
}