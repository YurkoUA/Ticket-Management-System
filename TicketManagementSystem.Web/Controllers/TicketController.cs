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
        public ActionResult Happy()
        {
            // TODO: Happy().

            var tickets = _ticketService.GetHappyTickets();
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Details(int id, bool partial = false)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<TicketDetailsModel>(ticket);
                return PartialView("DetailsPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Details",
                Controller = "Ticket",
                Param = id
            };

            ViewBag.Title = $"Квиток №{ticket.Number}";
            return View("Ticket", partialModel);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var viewModel = new TicketCreateModel
            {
                Colors = GetColorsList(),
                Series = GetSeriesList()
            };
            return View(viewModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Create(TicketCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PackageId == 0) model.PackageId = null;

                var createDTO = MapperInstance.Map<TicketCreateDTO>(model);
                var errors = _ticketService.Validate(createDTO);

                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var ticket = _ticketService.Create(createDTO);
                    return SuccessPartial($"Квиток №{model.Number} успішно додано!", Url.Action("Details", new { id = ticket.Id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, bool partial = false)
        {
            var ticket = _ticketService.GetEdit(id);

            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<TicketEditModel>(ticket);
                viewModel.Colors = GetColorsList();
                viewModel.Series = GetSeriesList();

                return PartialView("EditPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Edit",
                Controller = "Ticket",
                Param = id
            };

            ViewBag.Title = "Редагувати квиток";
            return View("Ticket", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Edit(TicketEditModel model)
        {
            if (ModelState.IsValid)
            {
                var editDTO = MapperInstance.Map<TicketEditDTO>(model);
                var errors = _ticketService.Validate(editDTO);

                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var id = _ticketService.Edit(editDTO).Id;
                    return SuccessPartial("Зміни збережено!", Url.Action("Details", new { id = id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool partial = false)
        {
            var ticket = _ticketService.GetById((int)id);

            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                return PartialView("DeletePartial", MapperInstance.Map<TicketDetailsModel>(ticket));
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Delete",
                Controller = "Ticket",
                Param = (int)id
            };

            ViewBag.Title = $"Видалити квиток №{ticket.Number}";
            return View("Ticket", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
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
                return SuccessPartial("Квиток видалено.");
            }
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult Move(int id, bool partial = false)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<TicketMoveModel>(ticket);
                viewModel.Packages = GetPackagesList(ticket.ColorId, ticket.SerialId, int.Parse(ticket.Number.First().ToString()));

                return PartialView("MovePartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "Move",
                Controller = "Ticket",
                Param = id
            };

            ViewBag.Title = $"Перемістити квиток №{ticket.Number}";
            return View("Ticket", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult Move(TicketMoveModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var errors = _ticketService.ValidateMoveToPackage(viewModel.Id, viewModel.PackageId);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _ticketService.MoveToPackage(viewModel.Id, viewModel.PackageId);
                    return SuccessPartial("Квиток успішно переміщено.");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult ChangeNumber(int id, bool partial = false)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<TicketChangeNumberModel>(ticket);
                return PartialView("ChangeNumberPartial", viewModel);
            }

            var partialModel = new PartialModel<int>
            {
                Action = "ChangeNumber",
                Controller = "Ticket",
                Param = id
            };

            ViewBag.Title = "Змінити номер квитка";
            return View("Ticket", partialModel);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public ActionResult ChangeNumber(TicketChangeNumberModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var errors = _ticketService.ValidateChangeNumber(viewModel.Id, viewModel.Number);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _ticketService.ChangeNumber(viewModel.Id, viewModel.Number);
                    return SuccessPartial($"Номер квитка змінено на №{viewModel.Number}.");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult GetPackageSelectPartial(int colorId, int serialId, string selectId, string selectName, int? number = null)
        {
            var viewModel = new SelectListModel
            {
                Id = selectId,
                Name = selectName,
                Options = GetPackagesList(colorId, serialId, number, true)
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

        private SelectList GetPackagesList(int colorId, int serialId, int? number = null, bool nullable = false)
        {
            IEnumerable<PackageDTO> packages = _packageService.GetPackages()
                .Where(p => (p.ColorId == null || p.ColorId == colorId) 
                    && (p.SerialId == null || p.SerialId == serialId) 
                    && p.IsOpened)
                .OrderBy(p => p.Name);

            if (number != null)
            {
                packages = packages.Where(p => p.FirstNumber == number || p.FirstNumber == null);
            }

            var packagesList = packages.ToList();

            if (nullable)
                packagesList.Add(new PackageDTO { Name = "(Немає)" });
            
            return new SelectList(packagesList, "Id", "SelectListOptionValue");
        }

        #endregion
    }
}