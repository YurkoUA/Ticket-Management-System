﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketController : ApplicationController
    {
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

        #region Index, Unallocated, Happy, Details

        [HttpGet, AllowAnonymous]
        public ActionResult Index(int page = 1)
        {
            const int ITEMS_ON_PAGE = 20;

            if (page < 1) page = 1;

            var pageInfo = new PageInfo(page, _ticketService.TotalCount, ITEMS_ON_PAGE);
            var tickets = _ticketService.GetTickets((page - 1) * ITEMS_ON_PAGE, ITEMS_ON_PAGE);

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets),
                PageInfo = pageInfo
            };
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Unallocated(int page = 1)
        {
            const int ITEMS_ON_PAGE = 30;

            if (page < 1) page = 1;

            var tickets = _ticketService.GetUnallocatedTickets((page - 1) * ITEMS_ON_PAGE, ITEMS_ON_PAGE);
            var pageInfo = new PageInfo(page, _ticketService.CountUnallocatedTickets(), ITEMS_ON_PAGE);

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets),
                PageInfo = pageInfo
            };
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Happy(int page = 1)
        {
            const int ITEMS_ON_PAGE = 30;

            if (page < 1) page = 1;

            var tickets = _ticketService.GetHappyTickets((page - 1) * ITEMS_ON_PAGE, ITEMS_ON_PAGE);
            var pageInfo = new PageInfo(page, _ticketService.CountHappyTickets(), ITEMS_ON_PAGE);

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets),
                PageInfo = pageInfo
            };
            return View(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Details(int id, bool partial = false)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<TicketDetailsModel>(ticket);

                var ticketsByNumber = _ticketService.CountByNumber(ticket.Number);

                if (ticketsByNumber > 1)
                    viewModel.Clones = ticketsByNumber - 1;

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

        #endregion

        [HttpGet, AllowAnonymous]
        public ActionResult Search()
        {
            throw new NotImplementedException();
        }

        #region CRUD

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new TicketCreateModel
            {
                Colors = GetColorsList(),
                Series = GetSeriesList()
            };
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
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

        [HttpGet]
        public ActionResult CreateMany()
        {
            var viewModel = new TicketCreateManyModel
            {
                CanSelectColor = true,
                CanSelectSerial = true,
                Colors = GetColorsList(),
                Series = GetSeriesList()
            };

            ViewBag.Title = "Створити декілька квитків";
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateManyInPackage(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsOpened) return HttpBadRequest();

            var viewModel = new TicketCreateManyModel
            {
                PackageId = package.Id,
                PackageName = package.Name
            };

            if (package.ColorId == null)
            {
                viewModel.CanSelectColor = true;
                viewModel.Colors = GetColorsList();
            }
            else
            {
                viewModel.ColorId = package.ColorId;
            }

            if (package.SerialId == null)
            {
                viewModel.CanSelectSerial = true;
                viewModel.Series = GetSeriesList();
            }
            else
            {
                viewModel.SerialId = package.SerialId;
            }

            ViewBag.Title = $"Створити декілька квитків у пачці \"{package.Name}\"";
            return View("CreateMany", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateMany(TicketCreateManyModel viewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
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

        [HttpPost, ValidateAntiForgeryToken]
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

        [HttpGet]
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

        [HttpPost, ValidateAntiForgeryToken]
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

        #endregion

        #region Move, ChangeNumber

        [HttpGet]
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

        [HttpPost, ValidateAntiForgeryToken]
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

        [HttpGet]
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

        [HttpPost, ValidateAntiForgeryToken]
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

        #endregion

        [HttpGet, AllowAnonymous]
        public ActionResult ClonesWith(int id)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null)
                return HttpNotFound();

            var tickets = _ticketService.GetByNumber(ticket.Number, ticket.Id);

            if (!tickets.Any())
                return HttpNotFound();

            var viewModel = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets);
            return PartialView(viewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Clones()
        {
            var viewModel = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(_ticketService.GetClones());
            return View(viewModel);
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

        #region PartialViews (Select)

        [HttpGet]
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
        
        [HttpGet]
        public ActionResult GetColorsPartial(string selectId, string selectName)
        {
            var viewModel = new SelectListModel
            {
                Id = selectId,
                Name = selectName,
                Options = GetColorsList()
            };
            return PartialView("SelectListPartial", viewModel);
        }

        [HttpGet]
        public ActionResult GetSeriesPartial(string selectId, string selectName)
        {
            var viewModel = new SelectListModel
            {
                Id = selectId,
                Name = selectName,
                Options = GetSeriesList()
            };
            return PartialView("SelectListPartial", viewModel);
        }

        #endregion
    }
}