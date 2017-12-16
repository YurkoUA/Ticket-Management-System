using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Enums;
using TicketManagementSystem.Web.Areas.Api.Controllers;
using static TicketManagementSystem.Web.Areas.Api.Models.Extensions;

namespace TicketManagementSystem.Web.Areas.Todo.Controllers
{
    [RoutePrefix("api/Todo")]
    [Authorize(Roles = "Admin")]
    public class TodoApiController : BaseApiController
    {
        private readonly ITodoService _todoService;
        private readonly ITodoValidationService _todoValidationService;

        public TodoApiController(ITodoService todoService, ITodoValidationService todoValidationService)
        {
            _todoService = todoService;
            _todoValidationService = todoValidationService;
        }

        [HttpGet, Route("GetAllTasks")]
        public IHttpActionResult GetAllTasks()
        {
            var tasks = _todoService.GetTasks();

            if (!tasks.Any())
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(tasks);
        }

        [HttpGet, Route("GetGroupedTasks/{take?}")]
        public IHttpActionResult GetGroupedTasks(int? take = null)
        {
            Dictionary<TaskStatus, IEnumerable<TodoTaskDTO>> tasks;

            if (take == null)
                tasks = _todoService.GetTasksGroupByStatus();
            else
                tasks = _todoService.GetTasksGroupByStatus((int)take);

            if (!tasks.Any())
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(tasks);
        }

        [HttpGet, Route("GetTasks/{status?}")]
        public IHttpActionResult GetTasks(string status)
        {
            TaskStatus taskStatus;

            if (!Enum.TryParse(status, out taskStatus))
                return BadRequest();

            var tasks = _todoService.GetTasks(taskStatus);

            if (!tasks.Any())
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(tasks);
        }

        [HttpGet, Route("Get/{id?}", Name = "ById")]
        public IHttpActionResult GetById(int id)
        {
            var task = _todoService.GetById(id);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost, Route("Create")]
        public dynamic Create([FromBody]TodoTaskDTO dto)
        {
            if (ModelState.IsValid)
            {
                var errors = _todoValidationService.ValidateCreate(dto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    var task = _todoService.Create(dto);
                    return Created(Url.Link("ById", new { id = task.Id }), task);
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut, Route("Edit")]
        public dynamic Edit(int id, [FromBody]TodoTaskDTO dto)
        {
            dto.Id = id;
            if (ModelState.IsValid)
            {
                var errors = _todoValidationService.ValidateEdit(dto);
                ToModelState(errors, ModelState);

                if (ModelState.IsValid)
                {
                    _todoService.Update(dto);
                    return Ok();
                }
            }
            return BadRequestWithErrors(ModelState);
        }

        [HttpPut, Route("SetStatus/{id?}")]
        public IHttpActionResult SetStatus(int id, [FromBody]string status)
        {
            var task = _todoService.GetById(id);

            if (task == null)
                return NotFound();

            TaskStatus taskStatus;

            if (!Enum.TryParse(status, out taskStatus))
                return BadRequest("Status is not valid.");

            _todoService.SetStatus(id, taskStatus);
            return Ok();
        }

        [HttpPut, Route("SetPriority/{id?}")]
        public IHttpActionResult SetPriority(int id, [FromBody]string priority)
        {
            var task = _todoService.GetById(id);

            if (task == null)
                return NotFound();

            TaskPriority taskPriority;

            if (!Enum.TryParse(priority, out taskPriority))
                return BadRequest("Priority is not valid.");

            _todoService.SetPriority(id, taskPriority);
            return Ok();
        }

        [HttpDelete, Route("Delete/{id?}")]
        public IHttpActionResult Delete(int id)
        {
            var task = _todoService.GetById(id);

            if (task == null)
                return NotFound();

            _todoService.Delete(id);
            return Ok();
        }
    }
}
