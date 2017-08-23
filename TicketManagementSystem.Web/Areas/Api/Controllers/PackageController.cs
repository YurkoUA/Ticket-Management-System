using System;
using System.Collections.Generic;
using System.Web.Http;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Enums;
using TicketManagementSystem.Business.Interfaces;
using static TicketManagementSystem.Web.Areas.Api.Models.Extensions;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Package")]
    ////[Authorize(Roles = "Admin")]
    public class PackageController : BaseApiController
    {
        private IPackageService _packageService;
        private ITicketService _ticketService;

        public PackageController(IPackageService packageService, ITicketService ticketService)
        {
            _packageService = packageService;
            _ticketService = ticketService;
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetAll(int skip = 0, int take = 30, PackagesFilter filter = PackagesFilter.All)
        {
            return OkOrNoContent(_packageService.GetPackages(skip, take, filter));
        }

        [HttpGet, AllowAnonymous, Route("Get/{id?}", Name = "PackageById")]
        public IHttpActionResult Get(int id)
        {
            return OkOrNotFound(_packageService.GetPackage(id));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetTickets(int id)
        {
            return OkOrNoContent(_packageService.GetPackageTickets(id, true));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetUnallocatedTickets(int id)
        {
            return OkOrNoContent(_ticketService.GetUnallocatedTickets(id));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Search(string name)
        {
            return OkOrNoContent(_packageService.FindByName(name));
        }

        [HttpPost]
        public dynamic Create([FromBody]PackageCreateDTO createDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(createDto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    var package = _packageService.CreatePackage(createDto);
                    //return Ok();
                    return Created(Url.Link("PackageById", new { id = package.Id }), new { Id = package.Id });
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPost]
        public dynamic CreateSpecial([FromBody]PackageSpecialCreateDTO createDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(createDto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    var package = _packageService.CreateSpecialPackage(createDto);
                    //return Ok();
                    return Created(Url.Link("PackageById", new { id = package.Id }), new { Id = package.Id });
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut]
        public dynamic Edit([FromBody]PackageEditDTO editDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(editDto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.EditPackage(editDto);
                    return Ok();
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut]
        public dynamic EditSpecial([FromBody]PackageSpecialEditDTO editDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(editDto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.EditSpecialPackage(editDto);
                    return Ok();
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpDelete]
        public dynamic Delete(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return NotFound();

            _packageService.Remove(id);
            return Ok();
        }

        [HttpPut]
        public dynamic Open(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return NotFound();

            if (package.IsOpened)
            {
                ToModelState(new[] { "Пачка й так відкрита." }, ModelState);
                return BadRequestWithErrors(ModelState);
            }

            _packageService.OpenPackage(id);
            return Ok();
        }

        [HttpPut]
        public dynamic Close(int id)
        {
            var package = _packageService.GetPackage(id);

            if (package == null)
                return NotFound();

            if (!package.IsOpened)
            {
                ToModelState(new[] { "Пачка й так закрита." }, ModelState);
                return BadRequestWithErrors(ModelState);
            }

            _packageService.ClosePackage(id);
            return Ok();
        }

        [HttpPut]
        public dynamic MakeSpecial([FromBody]PackageMakeSpecialDTO dto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(dto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.MakeSpecial(dto);
                    return Ok();
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut]
        public dynamic MakeDefault([FromBody]PackageMakeDefaultDTO dto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageService.Validate(dto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    _packageService.MakeDefault(dto);
                    return Ok();
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut]
        public dynamic MoveTickets(int id, [FromBody]IEnumerable<int> ticketsIds)
        {
            // TODO: MoveTickets.
            throw new NotImplementedException();
        }
    }
}
