using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TicketManagementSystem.Business.Infrastructure.Exceptions;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Data.EF.Models;
using TicketManagementSystem.Enumerations;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class SerialController : ApplicationController<Serial>
    {
        private ISerialService _serialService;

        public SerialController(ISerialService serialService)
        {
            _serialService = serialService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = MapperInstance.Map<IEnumerable<SerialIndexModel>>(_serialService.GetSeries());
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var serial = _serialService.GetSerial(id);

            if (serial == null)
                return HttpNotFound();

            var viewModel = MapperInstance.Map<SerialDetailsModel>(serial);

            if (Request.IsAjaxRequest())
                return PartialView("DetailsPartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Серія \"{serial.Name}\"";

            return View("Serial", PartialType.Details);
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
        public ActionResult Create(SerialCreateModel model)
        {
            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (_serialService.ExistsByName(model.Name))
            {
                ModelState.AddModelError("", $"Серія \"{model.Name}\" вже існує.");
                return ErrorPartial(ModelState);
            }

            var id = _serialService.Create(MapperInstance.Map<SerialCreateDTO>(model)).Id;
            return SuccessAlert($"Серію \"{model.Name}\" успішно додано!", Url.Action("Details", new { id = id }), "Переглянути");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            SerialEditDTO serial = _serialService.GetSerialEdit(id);

            if (serial == null)
                return HttpNotFound();

            var viewModel = MapperInstance.Map<SerialEditModel>(serial);

            if (Request.IsAjaxRequest())
                return PartialView("EditPartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Редагування серії \"{serial.Name}\"";

            return View("Serial", PartialType.Edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(SerialEditModel model)
        {
            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (!_serialService.IsNameFree(model.Id, model.Name))
            {
                ModelState.AddModelError("", $"Серія \"{model.Name}\" вже існує!");
                return ErrorPartial(ModelState);
            }

            _serialService.Edit(MapperInstance.Map<SerialEditDTO>(model));
            return SuccessAlert("Зміни збережено!");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            SerialDTO serial = _serialService.GetSerial((int)id);

            if (serial == null)
                return HttpNotFound();

            var viewModel = MapperInstance.Map<SerialDetailsModel>(serial);

            if (Request.IsAjaxRequest())
                return PartialView("DeletePartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Видалення серії \"{viewModel.Name}\"";

            return View("Serial", PartialType.Delete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var serial = _serialService.GetSerial(id);

            if (serial == null)
                return HttpNotFound();

            if (serial.PackagesCount == 0 && serial.TicketsCount == 0)
            {
                _serialService.Remove(serial.Id);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до неї належать!");
            return ErrorPartial(ModelState);
        }
    }
}