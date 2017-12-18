using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using TicketManagementSystem.Business;
using TicketManagementSystem.Business.AppSettings;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.Hubs;

namespace TicketManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;
        private readonly ITicketService2 _ticketService2;
        private readonly IPackageService _packageService;
        private readonly ISerialService _serialService;
        private readonly IColorService _colorService;
        private readonly ICacheService _cacheService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly ITicketValidationService _ticketValidationService;

        public TicketController(
            ITicketService ticketService,
            ITicketService2 ticketService2,
            IPackageService packageService,
            ISerialService serialService,
            IColorService colorService,
            ICacheService cacheService,
            IAppSettingsService appSettingsService,
            ITicketValidationService ticketValidationService)
        {
            _ticketService = ticketService;
            _ticketService2 = ticketService2;
            _packageService = packageService;
            _serialService = serialService;
            _colorService = colorService;
            _cacheService = cacheService;
            _appSettingsService = appSettingsService;
            _ticketValidationService = ticketValidationService;
        }

        #region Index, Unallocated, Happy, Details

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public ActionResult Index(int page = 1)
        {
            if (page < 1)
                page = 1;

            var itemsOnPage = _appSettingsService.ItemsOnPage;

            var pageInfo = new PageInfo(page, _ticketService.TotalCount, itemsOnPage);
            var tickets = _ticketService.GetTickets((page - 1) * itemsOnPage, itemsOnPage);

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets),
                PageInfo = pageInfo
            };

            if (Request.IsAjaxRequest())
                return PartialView("IndexPartial", viewModel);

            return View(viewModel);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public ActionResult Unallocated()
        {
            var unallocated = _ticketService.GetUnallocatedTickets();
            return View(MapperInstance.Map<IEnumerable<TicketDetailsModel>>(unallocated));
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public ActionResult Latest()
        {
            var latestTickets = _ticketService2.GetLatestTickets();
            return View(MapperInstance.Map<IEnumerable<TicketDetailsModel>>(latestTickets));
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public ActionResult Today(int timezoneOffset = 0)
        {
            var todayTickets = _ticketService2.GetTodayTickets(timezoneOffset);
            return View(MapperInstance.Map<IEnumerable<TicketDetailsModel>>(todayTickets));
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 10, Location = OutputCacheLocation.Client)]
        public ActionResult Happy(int page = 1)
        {
            if (page < 1)
                page = 1;

            IEnumerable<TicketDTO> happyTickets;

            if (_cacheService.Contains("HappyTickets"))
            {
                happyTickets = _cacheService.GetItem<IEnumerable<TicketDTO>>("HappyTickets");
            }
            else
            {
                happyTickets = _ticketService.GetHappyTickets();
                _cacheService.AddOrReplaceItem("HappyTickets", happyTickets, 5);
            }

            var itemsOnPage = _appSettingsService.ItemsOnPage;
            
            var pageInfo = new PageInfo(page, happyTickets.Count(), itemsOnPage);

            var viewModel = new TicketIndexModel
            {
                Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(happyTickets.Skip((page - 1) * itemsOnPage).Take(itemsOnPage)),
                PageInfo = pageInfo
            };

            if (Request.IsAjaxRequest())
                return PartialView("HappyPartial", viewModel);

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
        public ActionResult Filter(TicketFilterModel viewModel)
        {
            if (viewModel.Page < 1)
                viewModel.Page = 1;

            IEnumerable<TicketDTO> tickets;

            if (!viewModel.IsNull())
            {
                var itemsOnPage = _appSettingsService.ItemsOnPage;

                tickets = _ticketService.Filter(viewModel.FirstNumber, viewModel.ColorId, viewModel.SerialId);

                viewModel.Tickets = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(
                    tickets.Skip((viewModel.Page - 1) * itemsOnPage).Take(itemsOnPage));

                viewModel.PageInfo = new PageInfo(viewModel.Page, tickets.Count(), itemsOnPage);
            }

            viewModel.Colors = GetColorsList();
            viewModel.Series = GetSeriesList();

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

        [HttpGet, AllowAnonymous]
        public ActionResult Search(string number)
        {
            if (string.IsNullOrEmpty(number) && Regex.IsMatch(number, @"\d{6}"))
                ModelState.AddModelError("", "Необхідно вказати номер.");

            var tickets = _ticketService.GetByNumber(number, true);

            if (!tickets.Any())
                ModelState.AddModelError("", "Квитки не знайдено.");

            if (ModelState.IsValid)
            {
                return PartialView("SearchPartial", MapperInstance.Map<IEnumerable<TicketDetailsModel>>(tickets));
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, AllowAnonymous, OutputCache(Duration = 60, Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult SearchModal()
        {
            return PartialView("SearchModal");
        }

        #region CRUD

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
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
                var createDTO = MapperInstance.Map<TicketCreateDTO>(model);
                var errors = _ticketValidationService.Validate(createDTO);

                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    var ticket = _ticketService.Create(createDTO);
                    return SuccessPartial($"Квиток №{model.Number} успішно додано!", Url.Action("Details", new { id = ticket.Id }), "Переглянути");
                }
            }
            return ErrorPartial(ModelState);
        }

        [HttpGet, OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult CreateInPackage(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null) return HttpNotFound();
            if (!package.IsOpened) return HttpBadRequest();

            var viewModel = new TicketCreateInPackageModel
            {
                PackageId = package.Id,
                PackageName = package.Name
            };

            if (package.ColorId != null)
            {
                viewModel.ColorId = (int)package.ColorId;
            }
            else
            {
                viewModel.CanSelectColor = true;
                viewModel.Colors = GetColorsList();
            }

            if (package.SerialId != null)
            {
                viewModel.SerialId = (int)package.SerialId;
            }
            else
            {
                viewModel.CanSelectSerial = true;
                viewModel.Series = GetSeriesList();
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id, bool partial = false)
        {
            var ticket = _ticketService.GetEdit(id);


            if (ticket == null) return HttpNotFound();

            if (Request.IsAjaxRequest() || partial)
            {
                var viewModel = MapperInstance.Map<TicketEditModel>(ticket);

                if (viewModel.CanSelectColor)
                    viewModel.Colors = GetColorsList();

                if (viewModel.CanSelectSerial)
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
                var errors = _ticketValidationService.Validate(editDTO);

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

                TicketsHub.RemoveTicketsIds(new[] { id });
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
                var errors = _ticketValidationService.ValidateMoveToPackage(viewModel.Id, viewModel.PackageId);
                errors.ToModelState(ModelState);

                if (ModelState.IsValid)
                {
                    _ticketService.MoveToPackage(viewModel.Id, viewModel.PackageId);

                    if (viewModel.IsUnallocated)
                        TicketsHub.RemoveTicketsIds(new[] { viewModel.Id });

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
                var errors = _ticketValidationService.ValidateChangeNumber(viewModel.Id, viewModel.Number);
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

        [HttpGet, AllowAnonymous, OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
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

        [HttpGet, AllowAnonymous, OutputCache(Duration = 30, Location = OutputCacheLocation.Client)]
        public ActionResult Clones()
        {
            var viewModel = MapperInstance.Map<IEnumerable<TicketDetailsModel>>(_ticketService.GetClones());
            return View(viewModel);
        }

        #region SelectLists

        private SelectList GetColorsList()
        {
            IEnumerable<ColorDTO> colors;

            if (_cacheService.Contains("ColorSelectList"))
            {
                colors = _cacheService.GetItem<IEnumerable<ColorDTO>>("ColorSelectList");
            }
            else
            {
                colors = _colorService.GetColors().OrderBy(c => c.Name);
                _cacheService.AddOrReplaceItem("ColorSelectList", colors);
            }

            return new SelectList(colors, "Id", "Name");
        }

        private SelectList GetSeriesList()
        {
            IEnumerable<SerialDTO> series;

            if (_cacheService.Contains("SeriesSelectList"))
            {
                series = _cacheService.GetItem<IEnumerable<SerialDTO>>("SeriesSelectList");
            }
            else
            {
                series = _serialService.GetSeries().OrderBy(s => s.Name);
                _cacheService.AddOrReplaceItem("SeriesSelectList", series);
            }
            
            return new SelectList(series, "Id", "Name");
        }

        private SelectList GetPackagesList(int colorId, int serialId, int? number = null)
        {
            IEnumerable<PackageDTO> packages = _packageService.GetCompatiblePackages(colorId, serialId, number)
                .OrderBy(p => p.Name);

            return new SelectList(packages.ToList(), "Id", "SelectListOptionValue");
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
                Options = GetPackagesList(colorId, serialId, number),
                DefaultOption = "(Немає)"
            };
            return PartialView("SelectListPartial", viewModel);
        }

        [HttpGet, OutputCache(Duration = 10, Location = OutputCacheLocation.ServerAndClient)]
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

        [HttpGet, OutputCache(Duration = 10, Location = OutputCacheLocation.ServerAndClient)]
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