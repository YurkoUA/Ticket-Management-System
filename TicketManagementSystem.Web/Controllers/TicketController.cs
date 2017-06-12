using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    public class TicketController : ApplicationController
    {
        private const int TICKETS_ON_PAGE = 30;

        private ITicketService _ticketService;
        private IPackageService _packageService;
        private ISerialService _serialService;
        private IColorService _colorService;

        public TicketController(
            ITicketService ticketService,
            IPackageService packageService,
            ISerialService serialService,
            IColorService colorService)
        {
            _ticketService = ticketService;
            _packageService = packageService;
            _serialService = serialService;
            _colorService = colorService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            if (page < 1) page = 1;

            int totalPages = _ticketService.TotalCount;
            var pageInfo = new PageInfo(page, totalPages, TICKETS_ON_PAGE);
            var tickets = _ticketService.GetTickets((page - 1) * TICKETS_ON_PAGE, TICKETS_ON_PAGE);

            ViewBag.Title = $"Квитки (сторінка {page} з {pageInfo.TotalPages})";

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets),
                PageInfo = pageInfo
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Unallocated(int page = 1)
        {
            // TODO: Unallocated.
            if (page < 1) page = 1;

            int totalPages = _ticketService.TotalCount;
            var pageInfo = new PageInfo(page, totalPages, TICKETS_ON_PAGE);
            var tickets = _ticketService.GetUnallocatedTickets((page - 1) * TICKETS_ON_PAGE, TICKETS_ON_PAGE);

            ViewBag.Title = $"Нерозподілені квитки (сторінка {page} з {pageInfo.TotalPages})";

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets),
                PageInfo = pageInfo
            };
            return View("Index", viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null) return HttpNotFound();

            return View(MapperInstance.Map<TicketDetailsModel>(ticket));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var viewModel = new TicketCreateModel
            {
                Colors = GetColorsList(),
                Series = GetSeriesList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(TicketCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PackageId == 0) model.PackageId = null;

                if (!_colorService.ExistsById(model.ColorId))
                {
                    ModelState.AddModelError("", $"Кольору ID: {model.ColorId} не існує.");
                }
                else if (!_serialService.ExistsById(model.SerialId))
                {
                    ModelState.AddModelError("", $"Серії ID: {model.SerialId} не існує.");
                }
                else if (model.PackageId != null)
                {
                    var package = _packageService.GetPackage((int)model.PackageId);
                    var first = int.Parse(model.Number.First().ToString());

                    if (package == null)
                    {
                        ModelState.AddModelError("", $"Пачки ID: {model.PackageId} не існує.");
                    }
                    else if (package.ColorId != null && model.ColorId != package.ColorId)
                    {
                        ModelState.AddModelError("", "Неможливо додати квиток до пачки іншого кольору.");
                    }
                    else if (package.SerialId != null && model.SerialId != package.SerialId)
                    {
                        ModelState.AddModelError("", "Неможливо додати квиток до пачки іншої серії.");
                    }
                    else if (package.FirstNumber != null && first != package.FirstNumber)
                    {
                        ModelState.AddModelError("", $"До цієї пачки можна додавати лише квитки на цифру {package.FirstNumber}.");
                    }
                }
                if (!ModelState.IsValid) return ErrorPartial(ModelState);

                var ticket = _ticketService.Create(MapperInstance.Map<TicketCreateDTO>(model));
                return SuccessAlert($"Квиток {model.Number} успішно додано!", Url.Action("Details", new { id = ticket.Id }), "Переглянути");
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var ticket = _ticketService.GetEdit(id);

            if (ticket == null) return HttpNotFound();

            var viewModel = MapperInstance.Map<TicketEditModel>(ticket);
            viewModel.Colors = GetColorsList();
            viewModel.Series = GetSeriesList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(TicketEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_colorService.ExistsById(model.ColorId))
                {
                    ModelState.AddModelError("", $"Кольору ID: {model.ColorId} не існує.");
                }
                else if (!_serialService.ExistsById(model.SerialId))
                {
                    ModelState.AddModelError("", $"Серії ID: {model.SerialId} не існує.");
                }

                var ticket = _ticketService.GetById(model.Id);

                if (ticket.PackageId != null)
                {
                    var package = _packageService.GetPackage((int)ticket.PackageId);

                    if (package.ColorId != null && package.ColorId != model.ColorId)
                    {
                        ModelState.AddModelError("", "Неможливо додати квиток до пачки іншого кольору.");
                    }
                    else if (package.SerialId != null && package.SerialId != model.SerialId)
                    {
                        ModelState.AddModelError("", "Неможливо додати квиток до пачки іншої серії.");
                    }
                }

                if (!ModelState.IsValid) return ErrorPartial(ModelState);

                var id = _ticketService.Edit(MapperInstance.Map<TicketEditDTO>(model)).Id;
                return SuccessAlert("Зміни збережено!", Url.Action("Details", new { id = id }),"Переглянути");
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            var ticket = _ticketService.GetById((int)id);

            if (ticket == null) return HttpNotFound();

            return View(MapperInstance.Map<TicketDetailsModel>(ticket));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (!_ticketService.ExistsById(id))
            {
                ModelState.AddModelError("", $"Помилка! Не знайдено квиток ID: {id}");
                return ErrorPartial(ModelState);
            }
            else
            {
                _ticketService.Remove(id);
                return SuccessAlert("Квиток видалено.");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Move(int id)
        {
            // TODO: Move (GET).

            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Move(int ticketId, int packageId)
        {
            // TODO: Move (POST).

            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeNumber(int id)
        {
            // TODO: Change number (GET).

            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeNumber(int id, string number)
        {
            // TODO: Change number (POST).

            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult GetPackageSelectPartial(int colorId, int serialId, string selectId, string selectName, int? number = null)
        {
            var viewModel = new SelectListModel
            {
                Id = selectId,
                Name = selectName,
                Options = GetPackagesList(colorId, serialId, number)
            };
            return PartialView("SelectListPartial", viewModel);
        }

        #region SelectLists

        private SelectList GetColorsList()
        {
            var colors = _colorService.GetColors().OrderBy(c => c.Name);
            return new SelectList(colors, "Id", "Name");
        }

        private SelectList GetSeriesList()
        {
            var series = _serialService.GetSeries().OrderBy(s => s.Name);
            return new SelectList(series, "Id", "Name");
        }

        private SelectList GetPackagesList(int colorId, int serialId, int? number = null)
        {
            IEnumerable<PackageDTO> packages = _packageService.GetPackages()
                .Where(p => (p.ColorId == null || p.ColorId == colorId) && (p.SerialId == null || p.SerialId == serialId))
                .OrderBy(p => p.Name);

            if (number != null)
            {
                packages = packages.Where(p => p.FirstNumber == number || p.FirstNumber == null);
            }

            var packagesList = packages.ToList();
            packagesList.Add(new PackageDTO { Name = "(Немає)" });
            
            return new SelectList(packagesList, "Id", "SelectListOptionValue");
        }

        #endregion
    }
}