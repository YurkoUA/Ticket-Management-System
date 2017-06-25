﻿using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ColorController : ApplicationController
    {
        private IColorService _colorService;
        private IPackageService _packageService;

        public ColorController(IColorService colorService, IPackageService packageService)
        {
            _colorService = colorService;
            _packageService = packageService;
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 30, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var viewModel = MapperInstance.Map<IEnumerable<ColorIndexModel>>(_colorService.GetColors());
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Server)]
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

        [HttpGet, AllowAnonymous, OutputCache(Duration = 20, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult GetPackages(int id)
        {
            if (!_colorService.ExistsById(id)) return HttpNotFound();

            var packages = _packageService.GetPackagesByColor(id);
            
            return PartialView("~/Views/Package/PackagesModal.cshtml", MapperInstance.Map<IEnumerable<PackageDetailsModel>>(packages));
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ColorCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createDTO = MapperInstance.Map<ColorCreateDTO>(model);
                var errors = _colorService.Validate(createDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var id = _colorService.Create(createDTO).Id;
                    return SuccessPartial($"Колір \"{model.Name}\" успішно додано!",
                        Url.Action("Details", "Color", new { id = id }),
                        "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ColorEditModel model)
        {
            if (ModelState.IsValid)
            {
                var editDTO = MapperInstance.Map<ColorEditDTO>(model);
                var errors = _colorService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _colorService.Edit(editDTO);
                    return SuccessPartial("Зміни збережено!");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var color = _colorService.GetColor(id);

            if (color == null)
                return HttpNotFound();

            if (color.PackagesCount == 0 && color.TicketsCount == 0)
            {
                _colorService.Remove(id);
                return SuccessPartial("Колір видалено.");
            }
            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до нього належать!");
            return ErrorPartial(ModelState);
        }
    }
}