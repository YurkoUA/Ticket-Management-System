using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using static TicketManagementSystem.Web.Areas.Api.Models.Extensions;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Ticket")]
    [Authorize(Roles = "Admin")]
    public class TicketController : BaseApiController
    {
        private IPackageService _packageService;
        private ITicketService _ticketService;
        private ITicketService2 _ticketService2;

        public TicketController(ITicketService ticketService, IPackageService packageService, ITicketService2 ticketService2)
        {
            _ticketService = ticketService;
            _packageService = packageService;
            _ticketService2 = ticketService2;
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetCount()
        {
            return Ok(new
            {
                Total = _ticketService.TotalCount,
                Happy = _ticketService.CountHappyTickets(),
                Unallocated = _ticketService.CountUnallocatedTickets()
            });
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetAll(int skip = 0, int take = 30)
        {
            return OkOrNoContent(_ticketService.GetTickets(skip, take));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Happy(int skip = 0, int take = 30)
        {
            return OkOrNoContent(_ticketService.GetHappyTickets(skip, take));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Unallocated(int skip = 0, int take = 30)
        {
            return OkOrNoContent(_ticketService.GetUnallocatedTickets(skip, take));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Clones()
        {
            return OkOrNoContent(_ticketService.GetClones());
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Latest()
        {
            return OkOrNoContent(_ticketService2.GetLatestTickets());
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Filter([FromUri]TicketFilterModel filter)
        {
            const int ITEMS_ON_PAGE = 30;

            if (filter == null || filter?.IsNull() == true)
                return BadRequest();

            var tickets = _ticketService.Filter(filter.FirstNumber, filter.ColorId, filter.SerialId);
            return OkOrNoContent(tickets.Skip((filter.Page - 1) * ITEMS_ON_PAGE).Take(ITEMS_ON_PAGE));
        }

        [HttpGet, AllowAnonymous, Route("Search/{number}")]
        public IHttpActionResult Search(string number, bool partialMatches = false)
        {
            // TODO: Validate number.
            return OkOrNoContent(_ticketService.GetByNumber(number, partialMatches));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult ClonesWith(int id)
        {
            var number = _ticketService.GetById(id).Number;

            if (string.IsNullOrEmpty(number))
                return NotFound();

            return OkOrNoContent(_ticketService.GetByNumber(number, id));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult CompatiblePackages(int id)
        {
            var ticket = _ticketService.GetById(id);

            if (ticket == null)
                return NotFound();

            return OkOrNoContent(_packageService.GetCompatiblePackages(ticket.ColorId, ticket.SerialId, ticket.FirstNumber));
        }

        [HttpGet, AllowAnonymous, Route("Get/{id?}", Name = "TicketById")]
        public IHttpActionResult Get(int id)
        {
            return OkOrNotFound(_ticketService.GetById(id));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetRandom()
        {
            return OkOrNoContent(_ticketService.GetRandomTicket());
        }

        [HttpPost]
        public dynamic Create([FromBody]TicketCreateDTO createDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _ticketService.Validate(createDto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    var ticket = _ticketService.Create(createDto);
                    //return Ok();
                    return Created(Url.Link("TicketById", new { id = ticket.Id }), new { Id = ticket.Id });
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPost]
        public dynamic CreateMany([FromBody]IEnumerable<TicketCreateDTO> tickets)
        {
            // TODO: CreateMany.
            throw new NotImplementedException();
        }

        [HttpPut]
        public dynamic Edit([FromBody]TicketEditDTO editDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _ticketService.Validate(editDto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    _ticketService.Edit(editDto);
                    return Ok();
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpDelete]
        public dynamic Delete(int id)
        {
            if (!_ticketService.ExistsById(id))
                return NotFound();

            _ticketService.Remove(id);
            return Ok();
        }

        [HttpPut]
        public dynamic Move(int id, [FromBody]int packageId)
        {
            var errors = _ticketService.ValidateMoveToPackage(id, packageId);
            ToModelState(errors, ModelState);

            if (ModelState.IsValid)
            {
                _ticketService.MoveToPackage(id, packageId);
                return Ok();
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut]
        public dynamic ChangeNumber(int id, [FromBody]string number)
        {
            var errors = _ticketService.ValidateChangeNumber(id, number);
            ToModelState(errors, ModelState);

            if (ModelState.IsValid)
            {
                _ticketService.ChangeNumber(id, number);
                return Ok();
            }
            return BadRequestWithErrors(ModelState);
        }
    }
}
