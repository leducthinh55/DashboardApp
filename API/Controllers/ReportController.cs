using Application.Reports.Queries.GetReportTaskProgress;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet("task/completed")]
        public async Task<ActionResult> GetContact()
        {
            var command = new GetReportTaskProgressQuery()
            {
            };
            var resultModel = await _mediator.Send(command);
            if (resultModel.IsSucceeded())
            {
                return Ok(resultModel);
            }
            return BadRequest();
        }
    }
}
