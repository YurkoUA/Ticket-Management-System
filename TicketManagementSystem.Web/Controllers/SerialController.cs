using System.Collections.Generic;
using System.Web.Mvc;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class SerialController : ApplicationController
    {
        private ISerialService _serialService;
        private IPackageService _packageService;

        public SerialController(ISerialService serialService, IPackageService packageService)
        {
            _serialService = serialService;
            _packageService = packageService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = MapperInstance.Map<IEnumerable<SerialIndexModel>>(_serialService.GetSeries());
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id, bool partial = false)
        {
            var serial = _serialService.GetSerial(id);

            if (serial == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<SerialDetailsModel>(serial);
                return PartialView("DetailsPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Details",
                Controller = "Serial",
                Param = id
            };

            ViewBag.Title = $"Серія \"{serial.Name}\"";
            return View("Serial", partialModel);
        }

        [HttpGet]
        public ActionResult GetPackages(int id)
        {
            if (!_serialService.ExistsById(id)) return HttpNotFound();

            var packages = _packageService.GetPackagesBySerial(id);
            return PartialView("~/Views/Package/PackagesModal.cshtml", MapperInstance.Map<IEnumerable<PackageDetailsModel>>(packages));
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Create(SerialCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createDTO = MapperInstance.Map<SerialCreateDTO>(model);
                var errors = _serialService.Validate(createDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var id = _serialService.Create(createDTO).Id;
                    return SuccessPartial($"Серію \"{model.Name}\" успішно додано!", Url.Action("Details", new { id = id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, bool partial = false)
        {
            SerialEditDTO serial = _serialService.GetSerialEdit(id);

            if (serial == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<SerialEditModel>(serial);
                return PartialView("EditPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Edit",
                Controller = "Serial",
                Param = id
            };
            ViewBag.Title = $"Редагування серії \"{serial.Name}\"";

            return View("Serial", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Edit(SerialEditModel model)
        {
            if (ModelState.IsValid)
            {
                var editDTO = MapperInstance.Map<SerialEditDTO>(model);
                var errors = _serialService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _serialService.Edit(editDTO);
                    return SuccessPartial("Зміни збережено!");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool partial = false)
        {
            SerialDTO serial = _serialService.GetSerial((int)id);

            if (serial == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<SerialDetailsModel>(serial);
                return PartialView("DeletePartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Delete",
                Controller = "Serial",
                Param = (int)id
            };

            ViewBag.Title = $"Видалення серії \"{serial.Name}\"";
            return View("Serial", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var serial = _serialService.GetSerial(id);

            if (serial == null)
                return HttpNotFound();

            if (serial.PackagesCount == 0 && serial.TicketsCount == 0)
            {
                _serialService.Remove(serial.Id);
                return SuccessPartial("Серію видалено.");
            }
            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до неї належать!");
            return ErrorPartial(ModelState);
        }
    }
}