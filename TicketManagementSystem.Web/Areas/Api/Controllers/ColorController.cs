using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    public class ColorController : BaseApiController
    {
        private IColorService _colorService;
        private IPackageService _packageService;

        public ColorController(IColorService colorService, IPackageService packageService)
        {
            _colorService = colorService;
            _packageService = packageService;
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return OkOrNoContent(_colorService.GetColors());
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return OkOrNotFound(_colorService.GetColor(id));
        }

        [HttpGet]
        public IHttpActionResult GetPackages(int id)
        {
            return OkOrNoContent(_packageService.GetPackagesByColor(id));
        }
    }
}
