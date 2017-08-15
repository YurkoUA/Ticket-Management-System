using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Areas.Api.Controllers
{
    public class SerialController : BaseApiController
    {
        private ISerialService _serialService;
        private IPackageService _packageService;

        public SerialController(ISerialService serialService, IPackageService packageService)
        {
            _serialService = serialService;
            _packageService = packageService;
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return OkOrNoContent(_serialService.GetSeries());
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return OkOrNotFound(_serialService.GetSerial(id));
        }

        [HttpGet]
        public IHttpActionResult GetPackages(int id)
        {
            return OkOrNoContent(_packageService.GetPackagesBySerial(id));
        }
    }
}
