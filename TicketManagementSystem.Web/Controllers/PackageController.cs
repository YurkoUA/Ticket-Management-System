using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class PackageController : ApplicationController
    {
        // TODO: In "ToolbarPartial" hide delete button if package contains tickets.

        private ITicketService _ticketService;
        private IPackageService _packageService;
        private IColorService _colorService;
        private ISerialService _serialService;

        public PackageController(IPackageService packageService, 
            IColorService colorService, 
            ISerialService serialService,
            ITicketService ticketServive)
        {
            _packageService = packageService;
            _colorService = colorService;
            _serialService = serialService;
            _ticketService = ticketServive;
        }

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            if (page < 1)
                page = 1;
            
            const int ITEMS_ON_PAGE = 20;

            var pageInfo = new PageInfo(page, _packageService.TotalCount, ITEMS_ON_PAGE);
            var packages = _packageService.GetPackages((page - 1) * ITEMS_ON_PAGE, ITEMS_ON_PAGE);

            var viewModel = new PackageIndexModel
            {
                Packages = MapperInstance.Map<IEnumerable<PackageDetailsModel>>(packages),
                PageInfo = pageInfo
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id, bool partial = false)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<PackageDetailsModel>(package);
                viewModel.UnallocatedTicketsCount = _ticketService.CountUnallocatedByPackage(id);
                return PartialView("DetailsPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Details",
                Controller = "Package",
                Param = id
            };

            ViewBag.Title = $"Пачка \"{package.Name}\"";
            return View("Package", partialModel);
        }

        [HttpGet]
        public ActionResult Tickets(int id)
        {
            if (!_packageService.ExistsById(id)) return HttpNotFound();

            var packageName = _packageService.GetPackage(id).Name;
            var tickets = _packageService.GetPackageTickets(id, true);

            ViewBag.Title = $"Квитки з пачки \"{packageName}\"";
            return PartialView("TicketsPartial", MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets));
        }

        [HttpGet, Authorize(Roles = "Admin")]
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

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Create(PackageCreateDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var createDTO = MapperInstance.Map<PackageCreateDTO>(viewModel);
                var errors = _packageService.Validate(createDTO);

                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var packageId = _packageService.CreatePackage(createDTO).Id;
                    return SuccessPartial("Пачку успішно створено", Url.Action("Details", new { id = packageId }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult CreateSpecial(PackageCreateSpecialModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.ColorId == 0) viewModel.ColorId = null;
                if (viewModel.SerialId == 0) viewModel.SerialId = null;

                var createDTO = MapperInstance.Map<PackageSpecialCreateDTO>(viewModel);
                var errors = _packageService.Validate(createDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var packageId = _packageService.CreateSpecialPackage(createDTO).Id;
                    return SuccessPartial($"Пачку \"{viewModel.Name}\" успішно створено", Url.Action("Details", new { id = packageId }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, bool partial = false)
        {
            if (_packageService.GetPackage(id)?.IsSpecial == true)
                return RedirectToAction("EditSpecial", new { id = id });

            var editDTO = _packageService.GetPackageEdit(id);

            if (editDTO == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<PackageEditDefaultModel>(editDTO);
                viewModel.Colors = GetColorsSelectList();
                viewModel.Series = GetSeriesSelectList();

                return PartialView("EditPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Edit",
                Controller = "Package",
                Param = id
            };

            ViewBag.Title = "Редагувати пачку";
            return View("Package", partialModel);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult EditSpecial(int id, bool partial = false)
        {
            if (_packageService.GetPackage(id)?.IsSpecial == false)
                return RedirectToAction("Edit", new { id = id });

            var editSpecDTO = _packageService.GetSpecialPackageEdit(id);

            if (editSpecDTO == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<PackageEditSpecialModel>(editSpecDTO);
                viewModel.Colors = GetColorsSelectList(true);
                viewModel.Series = GetSeriesSelectList(true);

                if (viewModel.ColorId == null) viewModel.ColorId = 0;
                if (viewModel.SerialId == null) viewModel.SerialId = 0;

                return PartialView("EditSpecialPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "EditSpecial",
                Controller = "Package",
                Param = id
            };

            ViewBag.Title = $"Редагувати пачку \"{editSpecDTO.Name}\"";
            return View("Package", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Edit(PackageEditDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var editDTO = MapperInstance.Map<PackageEditDTO>(viewModel);
                var errors = _packageService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.EditPackage(editDTO);
                    return SuccessPartial("Пачку успішно відредаговано", Url.Action("Details", new { id = viewModel.Id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult EditSpecial(PackageEditSpecialModel viewModel)
        {
            // TODO: Validate changing Serial or Color.
            if (ModelState.IsValid)
            {
                if (viewModel.ColorId == 0) viewModel.ColorId = null;
                if (viewModel.SerialId == 0) viewModel.SerialId = null;

                var editDTO = MapperInstance.Map<PackageSpecialEditDTO>(viewModel);
                var errors = _packageService.Validate(editDTO);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.EditSpecialPackage(editDTO);
                    return SuccessPartial("Пачку успішно відредаговано", Url.Action("Details", new { id = viewModel.Id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, bool partial = false)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                return PartialView("DeletePartial", MapperInstance.Map<PackageDetailsModel>(package));
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Delete",
                Controller = "Package",
                Param = id
            };

            ViewBag.Title = $"Видалити пачку \"{package.Name}\"";
            return View("Package", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
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

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult Open(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (package.IsOpened) return HttpBadRequest();

            _packageService.OpenPackage(id);
            return RedirectToAction("Details", new { id = id, partial = true });
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult Close(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsOpened) return HttpBadRequest();

            _packageService.ClosePackage(id);
            return RedirectToAction("Details", new { id = id, partial = true });
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult MakeSpecial(int id, bool partial = false)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (package.IsSpecial) return HttpBadRequest();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<PackageMakeSpecialDTO>(package);
                return PartialView("MakeSpecialPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "MakeSpecial",
                Controller = "Package",
                Param = id
            };

            ViewBag.Title = "Зробити пачку звичайною";
            return View("Package", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult MakeSpecial(PackageMakeSpecialDTO viewModelDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(viewModelDto);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.MakeSpecial(viewModelDto);
                    return SuccessPartial("Пачку помічено як спеціальна.");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult MakeDefault(int id, bool partial = false)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsSpecial) return HttpBadRequest();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<PackageMakeDefaultModel>(package);
                viewModel.Colors = GetColorsSelectList();
                viewModel.Series = GetSeriesSelectList();

                return PartialView("MakeDefaultPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "MakeDefault",
                Controller = "Package",
                Param = id
            };

            ViewBag.Title = "Зробити пачку звичайною";
            return View("Package", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult MakeDefault(PackageMakeDefaultModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = MapperInstance.Map<PackageMakeDefaultDTO>(viewModel);
                var errors = _packageService.Validate(dto);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.MakeDefault(dto);
                    return SuccessPartial("Пачку помічено як звичайна.");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult MoveUnallocatedTickets(int id)
        {
            // id - packageId.
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsOpened) return HttpBadRequest();

            var tickets = _ticketService.GetUnallocatedTickets(id).ToArray();

            return PartialView("MoveUnallocatedPartial", MapperInstance.Map<TicketUnallocatedMoveModel[]>(tickets));
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult MoveUnallocatedTickets(int id, TicketUnallocatedMoveModel[] tickets)
        {
            var ticketsMove = tickets.Where(t => t.Move);

            if (!ticketsMove.Any())
            {
                ModelState.AddModelError("", "Ви не обрали жодного квитка.");
            }

            var toMoveIds = ticketsMove.Select(t => t.Id).ToArray();
            var errors = _ticketService.ValidateMoveFewToPackage(id, toMoveIds);
            errors.ToModelState(ModelState);

            if (ModelState.IsValid)
            {
                _ticketService.MoveFewToPackage(id, toMoveIds);
                return SuccessPartial($"Квитків переміщено {toMoveIds.Length}.");
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        public ActionResult MoveUnallocatedModal()
        {
            return PartialView("MoveUnallocatedModal");
        }

        #region SelectLists

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

        #endregion
    }
}