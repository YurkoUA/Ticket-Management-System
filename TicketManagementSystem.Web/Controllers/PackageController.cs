using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class PackageController : ApplicationController
    {
        // TODO: Make default/special.
        // TODO: Open/Close package.

        private IPackageService _packageService;
        private IColorService _colorService;
        private ISerialService _serialService;

        public PackageController(IPackageService packageService, IColorService colorService, ISerialService serialService)
        {
            _packageService = packageService;
            _colorService = colorService;
            _serialService = serialService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            if (page < 1)
                page = 1;

            const int PACKAGES_IN_PAGE = 20;

            var pageInfo = new PageInfo(page, _packageService.TotalCount);
            var packages = _packageService.GetPackages((page - 1) * PACKAGES_IN_PAGE, PACKAGES_IN_PAGE);

            var viewModel = MapperInstance.Map<IEnumerable<PackageIndexModel>>(packages);

            ViewBag.PageInfo = pageInfo;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return HttpNotFound();

            var viewModel = MapperInstance.Map<PackageDetailsModel>(package);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(bool special = false)
        {
            if (special)
            {
                return View("CreateSpecial", new PackageCreateSpecialModel { Colors = GetColorsSelectList(true), Series = GetSeriesSelectList(true) });
            }
            else
            {
                return View("CreateDefault", new PackageCreateDefaultModel { Colors = GetColorsSelectList(), Series = GetSeriesSelectList() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(PackageCreateDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_serialService.ExistsById(viewModel.SerialId))
                {
                    ModelState.AddModelError("", $"Серії ID: {viewModel.SerialId} не існує.");
                }

                if (!_colorService.ExistsById(viewModel.ColorId))
                {
                    ModelState.AddModelError("", $"Кольору ID: {viewModel.ColorId} не існує.");
                }
                var packageId = _packageService.CreatePackage(MapperInstance.Map<PackageCreateDTO>(viewModel)).Id;

                return SuccessAlert("Пачку успішно створено", Url.Action("Details", new { id = packageId }), "Переглянути");
            }
            return ErrorPartial(ModelState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateSpecial(PackageCreateSpecialModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.ColorId == 0)     viewModel.ColorId = null;
                if (viewModel.SerialId == 0)    viewModel.SerialId = null;

                if (_packageService.ExistsByName(viewModel.Name))
                {
                    ModelState.AddModelError("", $"Пачка з іменем \"{viewModel.Name}\" вже існує.");
                }

                else if (viewModel.SerialId != null && !_serialService.ExistsById((int)viewModel.SerialId))
                {
                    ModelState.AddModelError("", $"Серії ID: {viewModel.SerialId} не існує.");
                }

                else if (viewModel.ColorId != null && !_colorService.ExistsById((int)viewModel.ColorId))
                {
                    ModelState.AddModelError("", $"Кольору ID: {viewModel.ColorId} не існує.");
                }
                else
                {
                    var packageId = _packageService.CreateSpecialPackage(MapperInstance.Map<PackageSpecialCreateDTO>(viewModel)).Id;
                    return SuccessAlert($"Пачку \"{viewModel.Name}\" успішно створено", Url.Action("Details", new { id = packageId }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            if (_packageService.GetPackage(id)?.IsSpecial == true)
                return RedirectToAction("EditSpecial", new { id = id });

            var editDTO = _packageService.GetPackageEdit(id);

            if (editDTO == null)
                return HttpNotFound();

            var viewModel = MapperInstance.Map<PackageEditDefaultModel>(editDTO);
            viewModel.Colors = GetColorsSelectList();
            viewModel.Series = GetSeriesSelectList();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSpecial(int id)
        {
            var package = _packageService.GetSpecialPackageEdit(id);

            if (package == null)
                return HttpNotFound();

            var viewModel = MapperInstance.Map<PackageEditSpecialModel>(package);
            viewModel.Colors = GetColorsSelectList(true);
            viewModel.Series = GetSeriesSelectList(true);

            if (viewModel.ColorId == null) viewModel.ColorId = 0;
            if (viewModel.SerialId == null) viewModel.SerialId = 0;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(PackageEditDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_serialService.ExistsById(viewModel.SerialId))
                {
                    ModelState.AddModelError("", $"Серії ID: {viewModel.SerialId} не існує.");
                }

                if (!_colorService.ExistsById(viewModel.ColorId))
                {
                    ModelState.AddModelError("", $"Кольору ID: {viewModel.ColorId} не існує.");
                }
                _packageService.EditPackage(MapperInstance.Map<PackageEditDTO>(viewModel));

                return SuccessAlert("Пачку успішно відредаговано", Url.Action("Details", new { id = viewModel.Id }), "Переглянути");
            }
            return ErrorPartial(ModelState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditSpecial(PackageEditSpecialModel viewModel)
        {
            // TODO: Validate changing Serial or Color.
            if (ModelState.IsValid)
            {
                if (viewModel.ColorId == 0) viewModel.ColorId = null;
                if (viewModel.SerialId == 0) viewModel.SerialId = null;

                if (!_packageService.IsNameFree(viewModel.Id, viewModel.Name))
                {
                    ModelState.AddModelError("", $"Пачка \"{viewModel.Name}\" вже існує.");
                }

                if (viewModel.SerialId != null && !_serialService.ExistsById((int)viewModel.SerialId))
                {
                    ModelState.AddModelError("", $"Серії ID: {viewModel.SerialId} не існує.");
                }

                if (viewModel.ColorId != null && !_colorService.ExistsById((int)viewModel.ColorId))
                {
                    ModelState.AddModelError("", $"Кольору ID: {viewModel.ColorId} не існує.");
                }
                _packageService.EditSpecialPackage(MapperInstance.Map<PackageSpecialEditDTO>(viewModel));

                return SuccessAlert("Пачку успішно відредаговано", Url.Action("Details", new { id = viewModel.Id }), "Переглянути");
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return HttpNotFound();

            return View(MapperInstance.Map<PackageDetailsModel>(package));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            var package = _packageService.GetPackage((int)id);
            
            if (package == null)
                return HttpNotFound();

            if (package.TicketsCount == 0)
            {
                _packageService.Remove((int)id);
                return SuccessAlert("Пачку успішно видалено.");
            }
            else
            {
                ModelState.AddModelError("", "Неможливо видалити цю пачку, оскільки є квитки, що до неї належать.");
                return ErrorPartial(ModelState);
            }
        }

        private SelectList GetColorsSelectList(bool nullElement = false)
        {
            var colors = _colorService.GetColors().OrderBy(c => c.Name).ToList();

            if (nullElement)
                colors.Add(new ColorDTO { Name = "(Немає)" });

            return new SelectList(colors, "Id", "Name");
        }

        private SelectList GetSeriesSelectList(bool nullElement = false)
        {
            var series = _serialService.GetSeries().OrderBy(s => s.Name).ToList();

            if (nullElement)
                series.Add(new SerialDTO { Name = "(Немає)" });

            return new SelectList(series, "Id", "Name");
        }
    }
}