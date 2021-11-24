using Application.Common.Models;
using Application.Contacts.Commands.CreateContact;
using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Commands.DeleteTask;
using Application.Tasks.Queries.GetListTask;
using Application.Tasks.Queries.GetTaskDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResultModel>> Create(CreateTaskCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateTaskCommand command)
        {
            if (!id.Equals(command.Id))
            {
                return BadRequest();
            }

            var resultModel = await _mediator.Send(command);

            if (resultModel.IsSucceeded())
            {
                return NoContent();
            }
            return BadRequest(resultModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTaskDetail(Guid id)
        {
            var command = new GetTaskDetailQuery()
            {
                Id = id
            };
            var resultModel = await _mediator.Send(command);
            if (resultModel.IsSucceeded())
            {
                return Ok(resultModel);
            }
            return BadRequest(resultModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetTask(string keyword)
        {
            var command = new GetListTaskQuery()
            {
                Keyword = keyword
            };
            var resultModel = await _mediator.Send(command);
            if (resultModel.IsSucceeded())
            {
                return Ok(resultModel);
            }
            return BadRequest(resultModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultModel>> DeleteTask(Guid id)
        {
            var command = new DeleteTaskCommand()
            {
                Id = id
            };
            var resultModel = await _mediator.Send(command);
            if (resultModel.IsSucceeded())
            {
                return NoContent();
            }
            return BadRequest(resultModel);
        }
    }
}
