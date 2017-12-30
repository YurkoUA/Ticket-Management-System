using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.AppSettings;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Enums;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.Hubs;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PackageController : BaseController
    {
        private readonly ICacheService _cacheService;
        private readonly ITicketService _ticketService;
        private readonly IPackageService _packageService;
        private readonly IColorService _colorService;
        private readonly ISerialService _serialService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IPackageValidationService _packageValidationService;
        private readonly ITicketValidationService _ticketValidationService;

        public PackageController(IPackageService packageService, 
            IColorService colorService, 
            ISerialService serialService,
            ITicketService ticketServive,
            ICacheService cacheService,
            IAppSettingsService appSettingsService,
            IPackageValidationService packageValidationService,
            ITicketValidationService ticketValidationService)
        {
            _packageService = packageService;
            _colorService = colorService;
            _serialService = serialService;
            _ticketService = ticketServive;
            _cacheService = cacheService;
            _appSettingsService = appSettingsService;
            _packageValidationService = packageValidationService;
            _ticketValidationService = ticketValidationService;
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 20, Location = OutputCacheLocation.Client)]
        public ActionResult Index(int page = 1, PackagesFilter tab = PackagesFilter.All)
        {
            if (page < 1)
                page = 1;

            var itemsOnPage = _appSettingsService.ItemsOnPage;

            var packages = _packageService.GetPackages((page - 1) * itemsOnPage, itemsOnPage, tab);
            var count = _packageService.GetCount();
            var pageInfo = new PageInfo(page, count.Where(tab), itemsOnPage);

            var viewModel = new PackageIndexModel
            {
                Packages = Mapper.Map<IEnumerable<PackageDetailsModel>>(packages),
                PageInfo = pageInfo,
                Filter = tab,
                TotalPackages = count.Total,
                OpenedPackages = count.Opened,
                SpecialPackages = count.Special
            };

            if (Request.IsAjaxRequest())
                return PartialView("IndexPartial", viewModel);

            return View(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Details(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return HttpNotFound();

            var packageVM = Mapper.Map<PackageDetailsModel>(package);
            packageVM.UnallocatedTicketsCount = _ticketService.CountUnallocatedByPackage(id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("DetailsPartial", packageVM);
            }

            var partialModel = new PartialViewModel(id, packageVM, "DetailsPartial", $"Пачка \"{package}\"");
            return View("Package", partialModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Search(string name)
        {
            if (string.IsNullOrEmpty(name))
                ModelState.AddModelError("", "Необхідно ввести назву.");

            var packages = _packageService.FindByName(name);

            if (!packages.Any())
                ModelState.AddModelError("", "Пачки не знайдено");

            if (ModelState.IsValid)
            {
                return PartialView("SearchPartial", Mapper.Map<IEnumerable<PackageDetailsModel>>(packages));
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult SearchModal()
        {
            return PartialView("SearchModal");
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Filter(PackageFilterModel viewModel)
        {
            if (!viewModel.IsNull())
            {
                var packages = _packageService.Filter(Mapper.Map<PackageFilterDTO>(viewModel));
                viewModel.Packages = Mapper.Map<IEnumerable<PackageDetailsModel>>(packages);
            }

            viewModel.Colors = GetColorsSelectList();
            viewModel.Series = GetSeriesSelectList();

            if (viewModel.ColorId != null)
            {
                viewModel.ColorName = viewModel.Colors
                    .FirstOrDefault(i => i.Value.Equals(viewModel.ColorId.ToString()))
                    ?.Text;
            }

            if (viewModel.SerialId != null)
            {
                viewModel.SerialName = viewModel.Series
                    .FirstOrDefault(i => i.Value.Equals(viewModel.SerialId.ToString()))
                    ?.Text;
            }

            if (Request.IsAjaxRequest())
                return PartialView("FilterPartial", viewModel);

            return View(viewModel);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Tickets(int id)
        {
            if (!_packageService.ExistsById(id)) return HttpNotFound();

            var package = _packageService.GetPackage(id);
            var tickets = _packageService.GetPackageTickets(id, true);

            ViewBag.Title = $"Квитки з пачки \"{package}\"";
            ViewBag.PackageId = id;
            ViewBag.IsOpened = package.IsOpened;

            return View(Mapper.Map<IEnumerable<TicketDetailsModel>>(tickets));
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Create(bool special = false)
        {
            var colors = GetColorsSelectList();
            var series = GetSeriesSelectList();

            if (!special)
            {
                return View("CreateDefault", new PackageCreateDefaultModel { Colors = colors, Series = series });
            }
            else
            {
                return View("CreateSpecial", new PackageCreateSpecialModel { Colors = colors, Series = series });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(PackageCreateDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var createDTO = Mapper.Map<PackageCreateDTO>(viewModel);
                var errors = _packageValidationService.Validate(createDTO);

                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var packageId = _packageService.CreatePackage(createDTO).Id;
                    return SuccessPartial("Пачку успішно створено", Url.Action("Details", new { id = packageId }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateSpecial(PackageCreateSpecialModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var createDTO = Mapper.Map<PackageSpecialCreateDTO>(viewModel);
                var errors = _packageValidationService.Validate(createDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var packageId = _packageService.CreateSpecialPackage(createDTO).Id;
                    return SuccessPartial($"Пачку \"{viewModel.Name}\" успішно створено", Url.Action("Details", new { id = packageId }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (_packageService.GetPackage(id)?.IsSpecial == true)
                return RedirectToAction("EditSpecial", new { id });

            var editDTO = _packageService.GetPackageEdit(id);

            if (editDTO == null)
                return HttpNotFound();

            var packageVM = Mapper.Map<PackageEditDefaultModel>(editDTO);
            packageVM.Colors = GetColorsSelectList();
            packageVM.Series = GetSeriesSelectList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("EditPartial", packageVM);
            }

            var partialModel = new PartialViewModel(id, packageVM, "EditPartial", "Редагувати пачку");
            return View("Package", partialModel);
        }

        [HttpGet]
        public ActionResult EditSpecial(int id)
        {
            if (_packageService.GetPackage(id)?.IsSpecial == false)
                return RedirectToAction("Edit", new { id });

            var editSpecDTO = _packageService.GetSpecialPackageEdit(id);

            if (editSpecDTO == null)
                return HttpNotFound();

            var packageVM = Mapper.Map<PackageEditSpecialModel>(editSpecDTO);
            packageVM.Colors = GetColorsSelectList();
            packageVM.Series = GetSeriesSelectList();

            if (packageVM.ColorId == null) packageVM.ColorId = 0;
            if (packageVM.SerialId == null) packageVM.SerialId = 0;

            if (Request.IsAjaxRequest())
            {
                return PartialView("EditSpecialPartial", packageVM);
            }

            var partialModel = new PartialViewModel(id, packageVM, "EditSpecialPartial", $"Редагувати пачку \"{editSpecDTO.Name}\"");
            return View("Package", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(PackageEditDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var editDTO = Mapper.Map<PackageEditDTO>(viewModel);
                var errors = _packageValidationService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.EditPackage(editDTO);
                    return SuccessPartial("Пачку успішно відредаговано", Url.Action("Details", new { id = viewModel.Id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditSpecial(PackageEditSpecialModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var editDTO = Mapper.Map<PackageSpecialEditDTO>(viewModel);
                var errors = _packageValidationService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.EditSpecialPackage(editDTO);
                    return SuccessPartial("Пачку успішно відредаговано", Url.Action("Details", new { id = viewModel.Id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return HttpNotFound();

            var packageVM = Mapper.Map<PackageDetailsModel>(package);

            if (Request.IsAjaxRequest())
            {
                return PartialView("DeletePartial", packageVM);
            }

            var partialModel = new PartialViewModel(id, packageVM, "DeletePartial", $"Видалити пачку \"{package}\"");
            return View("Package", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            var package = _packageService.GetPackage((int)id);

            if (package == null)
                return HttpNotFound();

            if (package.TicketsCount == 0)
            {
                _packageService.Remove((int)id);
                return SuccessPartial("Пачку успішно видалено.");
            }
            else
            {
                ModelState.AddModelError("", "Неможливо видалити цю пачку, оскільки є квитки, що до неї належать.");
                return ErrorPartial(ModelState);
            }
        }

        [HttpPost]
        public ActionResult Open(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (package.IsOpened) return HttpBadRequest();

            _packageService.OpenPackage(id);
            return HttpSuccess();
        }

        [HttpPost]
        public ActionResult Close(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsOpened) return HttpBadRequest();

            _packageService.ClosePackage(id);
            return HttpSuccess();
        }

        [HttpGet]
        public ActionResult MakeSpecial(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (package.IsSpecial) return HttpBadRequest();

            var packageVM = Mapper.Map<PackageMakeSpecialDTO>(package);

            if (Request.IsAjaxRequest())
            {
                return PartialView("MakeSpecialPartial", packageVM);
            }

            var partialModel = new PartialViewModel(id, packageVM, "MakeSpecialPartial", "Зробити пачку звичайною");
            return View("Package", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MakeSpecial(PackageMakeSpecialDTO viewModelDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageValidationService.Validate(viewModelDto);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.MakeSpecial(viewModelDto);
                    return SuccessPartial("Пачку помічено як спеціальна.");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult MakeDefault(int id, bool partial = false)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsSpecial) return HttpBadRequest();

            var packageVM = Mapper.Map<PackageMakeDefaultModel>(package);
            packageVM.Colors = GetColorsSelectList();
            packageVM.Series = GetSeriesSelectList();

            if (Request.IsAjaxRequest() || partial)
            {
                return PartialView("MakeDefaultPartial", packageVM);
            }

            var partialModel = new PartialViewModel(id, packageVM, "MakeDefaultPartial", "Зробити пачку звичайною");
            return View("Package", partialModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MakeDefault(PackageMakeDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = Mapper.Map<PackageMakeDefaultDTO>(viewModel);
                var errors = _packageValidationService.Validate(dto);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.MakeDefault(dto);
                    return SuccessPartial("Пачку помічено як звичайна.");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult MoveUnallocatedTickets(int id)
        {
            // id - packageId.
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsOpened) return HttpBadRequest();

            var tickets = _ticketService.GetUnallocatedTickets(id).ToArray();

            return PartialView("MoveUnallocatedPartial", Mapper.Map<TicketUnallocatedMoveModel[]>(tickets));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MoveUnallocatedTickets(int id, IEnumerable<TicketUnallocatedMoveModel> tickets)
        {
            var ticketsMove = tickets.Where(t => t.Move);

            if (!ticketsMove.Any())
            {
                ModelState.AddModelError("", "Ви не обрали жодного квитка.");
            }

            var toMoveIds = ticketsMove.Select(t => t.Id).ToArray();
            var errors = _ticketValidationService.ValidateMoveFewToPackage(id, toMoveIds);
            errors.ToModelState(ModelState);

            if (ModelState.IsValid)
            {
                _ticketService.MoveFewToPackage(id, toMoveIds);

                TicketsHub.RemoveTicketsIds(ticketsMove.Select(t => t.Id));
                return SuccessPartial($"Квитків переміщено {toMoveIds.Length}.");
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult MoveUnallocatedModal()
        {
            return PartialView("MoveUnallocatedModal");
        }

        #region SelectLists

        private SelectList GetColorsSelectList()
        {
            List<ColorDTO> colors;

            if (_cacheService.Contains("ColorSelectList"))
            {
                colors = _cacheService.GetItem<IEnumerable<ColorDTO>>("ColorSelectList").ToList();
            }
            else
            {
                colors = _colorService.GetColors().OrderBy(c => c.Name).ToList();
                _cacheService.AddOrReplaceItem("ColorSelectList", colors.AsEnumerable());
            }

            return new SelectList(colors, "Id", "Name");
        }

        private SelectList GetSeriesSelectList()
        {
            List<SerialDTO> series;

            if (_cacheService.Contains("SerialSelectList"))
            {
                series = _cacheService.GetItem<IEnumerable<SerialDTO>>("SerialSelectList").ToList();
            }
            else
            {
                series = _serialService.GetSeries().OrderBy(s => s.Name).ToList();
                _cacheService.AddOrReplaceItem("SerialSelectList", series.AsEnumerable());
            }

            return new SelectList(series, "Id", "Name");
        }

        #endregion
    }
}