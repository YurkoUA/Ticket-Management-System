using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ColorController : BaseController
    {
        private readonly ICacheService _cacheService;
        private readonly IColorService _colorService;
        private readonly IPackageService _packageService;
        private readonly IColorValidationService _colorValidationService;

        public ColorController(IColorService colorService, 
            IPackageService packageService, 
            ICacheService cacheServ,
            IColorValidationService colorValidationService)
        {
            _colorService = colorService;
            _packageService = packageService;
            _cacheService = cacheServ;
            _colorValidationService = colorValidationService;
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 30, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var viewModel = MapperInstance.Map<IEnumerable<ColorIndexModel>>(_colorService.GetColors());
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10)]
        public ActionResult Details(int id)
        {
            var color = _colorService.GetColor(id);

            if (color == null)
                return HttpNotFound();

            var colorVM = MapperInstance.Map<ColorDetailsModel>(color);

            if (Request.IsAjaxRequest())
            {
                return PartialView("DetailsPartial", colorVM);
            }

            var partialModel = new PartialViewModel(id, colorVM, "DetailsPartial", $"Колір \"{color.Name}\"");
            return View("Color", partialModel);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 20, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult GetPackages(int id)
        {
            if (!_colorService.ExistsById(id)) 
                return HttpNotFound();

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
                var errors = _colorValidationService.Validate(createDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var id = _colorService.Create(createDTO).Id;
                    _cacheService.DeleteItem("ColorSelectList");

                    return SuccessPartial($"Колір \"{model.Name}\" успішно додано!",
                        Url.Action("Details", "Color", new { id = id }),
                        "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var color = _colorService.GetColorEdit(id);

            if (color == null)
                return HttpNotFound();

            var colorVM = MapperInstance.Map<ColorEditModel>(color);

            if (Request.IsAjaxRequest())
            {
                return PartialView("EditPartial", colorVM);
            }
            
            var partialModel = new PartialViewModel(id, colorVM, "EditPartial", $"Редагування кольору \"{color.Name}\"");
            return View("Color", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ColorEditModel model)
        {
            if (ModelState.IsValid)
            {
                var editDTO = MapperInstance.Map<ColorEditDTO>(model);
                var errors = _colorValidationService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _colorService.Edit(editDTO);
                    _cacheService.DeleteItem("ColorSelectList");

                    return SuccessPartial("Зміни збережено!");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var color = _colorService.GetColor((int)id);

            if (color == null)
                return HttpNotFound();

            var colorVM = MapperInstance.Map<ColorDetailsModel>(color);

            if (Request.IsAjaxRequest())
            {
                return PartialView("DeletePartial", colorVM);
            }
            
            var partialModel = new PartialViewModel((int)id, colorVM, "DeletePartial", $"Видалення кольору \"{color.Name}\"");
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
                _cacheService.DeleteItem("ColorSelectList");

                return SuccessPartial("Колір видалено.");
            }
            ModelState.AddModelError("", "Неможливо видалити цей колір, бо є квитки та пачки, що до нього належать!");
            return ErrorPartial(ModelState);
        }
    }
}