using System;
using System.Linq;
using System.Web.Mvc;
using TicketManagementSystem.Business.Infrastructure.Exceptions;
using TicketManagementSystem.Business.Services;
using TicketManagementSystem.Data.Models;
using TicketManagementSystem.Enumerations;
using TicketManagementSystem.Web.Filters;
using TicketManagementSystem.Web.ViewModels.Serial;

namespace TicketManagementSystem.Web.Controllers
{
    public class SerialController : ApplicationController<Serial>
    {
        private SerialService _service;

        public SerialController()
        {
            _service = SerialService.GetInstance();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var serials = _service.Repository.GetAll();

            return View(serials.Select(m => new SerialIndexModel
            {
                Id = m.Id,
                Name = m.Name,
                Note = m.Note,
                PackagesCount = m.Packages.Count,
                TicketsCount = m.Tickets.Count
            }));
        }

        [HttpGet]
        public ActionResult Details(int? id, bool partial = false)
        {
            if (id == null)
                return RedirectToAction("Index");

            Serial serial = _service.Repository.GetById((int)id);

            if (serial == null)
                return NotFound();

            var viewModel = new SerialDetailsModel
            {
                Id = serial.Id,
                Name = serial.Name,
                Note = serial.Note,
                PackagesCount = serial.Packages.Count,
                TicketsCount = serial.Tickets.Count
            };

            if (partial)
                return PartialView("DetailsPartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Серія \"{serial.Name}\"";

            return View("Serial", PartialType.Details);
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
        public ActionResult Create(SerialCreateModel model)
        {
            if (model == null)
                throw new ModelIsNullException();

            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (_service.Repository.Contains(m => m.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                ModelState.AddModelError("", $"Серія \"{model.Name}\" вже існує.");
                return ErrorPartial(ModelState);
            }

            var id = _service.CreateSerial(model.Name, model.Note, model.RowVersion).Id;

            return SuccessAlert($"Серію \"{model.Name}\" успішно додано!", Url.Action("Details", new { id = id }), "Переглянути");
        }

        [HttpGet]
        [Admin]
        public ActionResult Edit(int? id, bool partial = false)
        {
            if (id == null)
                return RedirectToAction("Index");

            Serial serial = _service.Repository.GetById((int)id);

            if (serial == null)
                return NotFound();

            var viewModel = new SerialEditModel
            {
                Id = serial.Id,
                Name = serial.Name,
                Note = serial.Note,
                RowVersion = serial.RowVersion,
                CanBeDeleted = !serial.Packages.Any() && !serial.Tickets.Any()
            };

            if (partial)
                return PartialView("EditPartial", viewModel);

            ViewBag.ViewModel = viewModel;
            ViewBag.Title = $"Редагування серії \"{serial.Name}\"";

            return View("Serial", PartialType.Edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Admin]
        public ActionResult Edit(SerialEditModel model)
        {
            if (model == null)
                throw new ModelIsNullException();

            if (!ModelState.IsValid)
                return ErrorPartial(ModelState);

            if (!_service.CanBeEdited(model.Id, model.Name))
            {
                ModelState.AddModelError("", $"Серія \"{model.Name}\" вже існує!");
                return ErrorPartial(ModelState);
            }

            _service.EditSerial(model.Id, model.Name, model.Note, model.RowVersion);
            return SuccessAlert("Зміни збережено!");
        }

        [HttpGet]
        [Admin]
        public ActionResult Delete(int? id, bool partial = false)
        {
            if (id == null)
                return RedirectToAction("Index");

            Serial serial = _service.Repository.GetById((int)id);

            if (serial == null)
                return NotFound();

            if (partial)
                return PartialView("DeletePartial", serial);

            ViewBag.ViewModel = serial;
            ViewBag.Title = $"Видалення серії \"{serial.Name}\"";

            return View("Serial", PartialType.Delete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Admin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            Serial serial = _service.Repository.GetById((int)id);

            if (!serial.Packages.Any() && !serial.Tickets.Any())
            {
                _service.RemoveSerial(serial.Id);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до неї належать!");
            return ErrorPartial(ModelState);
        }
    }
}