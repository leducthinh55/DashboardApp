using Application.Reports.Queries.GetReportTaskProgress;
using DashboardApp.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ApiControllerBase
    {
        [HttpGet("task/completed")]
        public async Task<ActionResult> GetContact()
        {
            var command = new GetReportTaskProgressQuery()
            {
            };
            var resultModel = await Mediator.Send(command);
            if (resultModel.IsSucceeded())
            {
                return Ok(resultModel);
            }
            return BadRequest();
        }
    }
}
