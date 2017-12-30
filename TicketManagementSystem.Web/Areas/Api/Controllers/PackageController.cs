using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Enums;
using TicketManagementSystem.Business.Interfaces;
using static TicketManagementSystem.Web.Areas.Api.Models.Extensions;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/Package")]
    [Authorize(Roles = "Admin")]
    public class PackageController : BaseApiController
    {
        private readonly IPackageService _packageService;
        private readonly ITicketService _ticketService;
        private readonly IPackageValidationService _packageValidationService;

        public PackageController(IPackageService packageService, ITicketService ticketService, IPackageValidationService packageValidationService)
        {
            _packageService = packageService;
            _ticketService = ticketService;
            _packageValidationService = packageValidationService;
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetCount() => Ok(_packageService.GetCount());

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetAll(int skip = 0, int take = 30, PackagesFilter filter = PackagesFilter.All)
        {
            return OkOrNoContent(_packageService.GetPackages(skip, take, filter));
        }

        [HttpGet, AllowAnonymous]
        public IHttpActionResult Filter([FromUri]PackageFilterModel filter)
        {
            if (filter == null || filter?.IsNull() == true)
                return BadRequest();

            return OkOrNoContent(_packageService.Filter(MapperInstance.Map<PackageFilterDTO>(filter)));
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

        [HttpGet, AllowAnonymous]
        public IHttpActionResult GetCompatiblePackages(int colorId, int serialId, int? firstNumber)
        {
            var packages = _packageService.GetCompatiblePackages(colorId, serialId, firstNumber)?
                .OrderBy(p => p.IsSpecial);

            return OkOrNoContent(packages);
        }

        [HttpPost]
        public dynamic Create([FromBody]PackageCreateDTO createDto)
        {
            if (ModelState.IsValid)
            {
                var errors = _packageValidationService.Validate(createDto);
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
                var errors = _packageValidationService.Validate(createDto);
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
                var errors = _packageValidationService.Validate(editDto);
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
                var errors = _packageValidationService.Validate(editDto);
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
                var errors = _packageValidationService.Validate(dto);
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
                var errors = _packageValidationService.Validate(dto);
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
