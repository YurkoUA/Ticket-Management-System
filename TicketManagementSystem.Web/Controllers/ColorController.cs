using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TicketManagementSystem.Business.Infrastructure.Exceptions;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Data.EF.Models;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class ColorController : ApplicationController<Color>
    {
        private IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = MapperInstance.Map<IEnumerable<ColorIndexModel>>(_colorService.GetColors());
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id, bool partial = false)
        {
            var color = _colorService.GetColor(id);

            if (color == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<ColorDetailsModel>(color);
                return PartialView("DetailsPartial", viewModel);
            }
            
            ViewBag.Title = $"Колір \"{color.Name}\"";

            var partialModel = new PartialModel<int>
            {
                Action = "Details",
                Controller = "Color",
                Param = id
            };
            return View("Color", partialModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ColorCreateModel model)
        {
            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (_colorService.ExistsByName(model.Name))
            {
                ModelState.AddModelError("", $"Колір \"{model.Name}\" вже існує.");
                return ErrorPartial(ModelState);
            }
            var id = _colorService.Create(MapperInstance.Map<ColorCreateDTO>(model)).Id;

            return SuccessAlert($"Колір \"{model.Name}\" успішно додано!",
                Url.Action("Details", "Color", new { id = id }),
                "Переглянути");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, bool partial = false)
        {
            ColorEditDTO color = _colorService.GetColorEdit(id);

            if (color == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<ColorEditModel>(color);
                return PartialView("EditPartial", viewModel);
            }
            
            ViewBag.Title = $"Редагування кольору \"{color.Name}\"";

            var partialModel = new PartialModel<int>
            {
                Action = "Edit",
                Controller = "Color",
                Param = id
            };
            return View("Color", partialModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ColorEditModel model)
        {
            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (!_colorService.IsNameFree(model.Id, model.Name))
            {
                ModelState.AddModelError("", $"Колір \"{model.Name}\" вже існує!");
                return ErrorPartial(ModelState);
            }

            _colorService.Edit(MapperInstance.Map<ColorEditDTO>(model));
            return SuccessAlert("Зміни збережено!");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool partial = false)
        {
            ColorDTO color = _colorService.GetColor((int)id);

            if (color == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<ColorDetailsModel>(color);
                return PartialView("DeletePartial", viewModel);
            }
            
            ViewBag.Title = $"Видалення кольору \"{color.Name}\"";

            var partialModel = new PartialModel<int>
            {
                Action = "Delete",
                Controller = "Color",
                Param = (int)id
            };
            return View("Color", partialModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var color = _colorService.GetColor(id);

            if (color == null)
                return HttpNotFound();

            if (color.PackagesCount == 0 && color.TicketsCount == 0)
            {
                _colorService.Remove(id);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до нього належать!");
            return ErrorPartial(ModelState);
        }
    }
}