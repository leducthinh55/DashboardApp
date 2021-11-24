using Application.Common.Models;
using Application.Dashboards.Commands.CreateDashboard;
using Application.Dashboards.Commands.DeleteDashboard;
using Application.Dashboards.Queries.GetDashboardDetail;
using Application.Dashboards.Queries.GetListDashboard;
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
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResultModel>> Create(CreateDashboardCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateDashboardCommand command)
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
        public async Task<ActionResult> GetDashboardDetail(Guid id)
        {
            var command = new GetDashboardDetailQuery()
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
        public async Task<ActionResult> GetDashboard(string keyword)
        {
            var command = new GetListDashboardQuery()
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
        public async Task<ActionResult<ResultModel>> DeleteDashboard(Guid id)
        {
            var command = new DeleteDashboardCommand()
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
