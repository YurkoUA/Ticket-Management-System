using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SerialController : BaseController
    {
        private readonly ICacheService _cacheService;
        private readonly ISerialService _serialService;
        private readonly IPackageService _packageService;
        private readonly ISerialValidationService _serialValidationService;

        public SerialController(ISerialService serialService, 
            IPackageService packageService, 
            ICacheService cacheServ,
            ISerialValidationService serialValidationService)
        {
            _serialService = serialService;
            _packageService = packageService;
            _cacheService = cacheServ;
            _serialValidationService = serialValidationService;
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 30, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var viewModel = MapperInstance.Map<IEnumerable<SerialIndexModel>>(_serialService.GetSeries());
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Details(int id)
        {
            var serial = _serialService.GetSerial(id);

            if (serial == null)
                return HttpNotFound();

            var serialVM = MapperInstance.Map<SerialDetailsModel>(serial);

            if (Request.IsAjaxRequest())
            {
                return PartialView("DetailsPartial", serialVM);
            }

            var partialModel = new PartialViewModel(id, serialVM, "DetailsPartial", $"Серія \"{serial.Name}\"");
            return View("Serial", partialModel);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 20, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult GetPackages(int id)
        {
            if (!_serialService.ExistsById(id)) return HttpNotFound();

            var packages = _packageService.GetPackagesBySerial(id);
            return PartialView("~/Views/Package/PackagesModal.cshtml", MapperInstance.Map<IEnumerable<PackageDetailsModel>>(packages));
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(SerialCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createDTO = MapperInstance.Map<SerialCreateDTO>(model);
                var errors = _serialValidationService.Validate(createDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var id = _serialService.Create(createDTO).Id;
                    _cacheService.DeleteItem("SeriesSelectList");

                    return SuccessPartial($"Серію \"{model.Name}\" успішно додано!", Url.Action("Details", new { id = id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var serial = _serialService.GetSerialEdit(id);

            if (serial == null)
                return HttpNotFound();

            var serialVM = MapperInstance.Map<SerialEditModel>(serial);

            if (Request.IsAjaxRequest())
            {
                return PartialView("EditPartial", serialVM);
            }

            var partialModel = new PartialViewModel(id, serialVM, "EditPartial", $"Редагування серії \"{serial.Name}\"");
            return View("Serial", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(SerialEditModel model)
        {
            if (ModelState.IsValid)
            {
                var editDTO = MapperInstance.Map<SerialEditDTO>(model);
                var errors = _serialValidationService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _serialService.Edit(editDTO);
                    _cacheService.DeleteItem("SeriesSelectList");

                    return SuccessPartial("Зміни збережено!");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var serial = _serialService.GetSerial((int)id);

            if (serial == null)
                return HttpNotFound();

            var serialVM = MapperInstance.Map<SerialDetailsModel>(serial);

            if (Request.IsAjaxRequest())
            {
                return PartialView("DeletePartial", serialVM);
            }

            var partialModel = new PartialViewModel((int)id, serialVM, "DeletePartial", $"Видалення серії \"{serial.Name}\"");
            return View("Serial", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var serial = _serialService.GetSerial(id);

            if (serial == null)
                return HttpNotFound();

            if (serial.PackagesCount == 0 && serial.TicketsCount == 0)
            {
                _serialService.Remove(serial.Id);
                _cacheService.DeleteItem("SeriesSelectList");

                return SuccessPartial("Серію видалено.");
            }
            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до неї належать!");
            return ErrorPartial(ModelState);
        }
    }
}