using Application.Common.Models;
using Application.Contacts.Commands.CreateContact;
using Application.Contacts.Commands.DeleteContact;
using Application.Contacts.Queries.ExportContact;
using Application.Contacts.Queries.GetContactDetail;
using Application.Contacts.Queries.GetListContact;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        /// <summary>
        /// create contact
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResultModel>> Create(CreateContactCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// update contact
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateContactCommand command)
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

        /// <summary>
        /// get contact detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetContactDetail(Guid id)
        {
            var command = new GetContactDetailQuery()
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

        /// <summary>
        /// get list contact
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetContact(string keyword)
        {
            var command = new GetListContactQuery()
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

        /// <summary>
        /// delete list contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultModel>> DeleteContact(Guid id)
        {
            var command = new DeleteContactCommand()
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

        /// <summary>
        /// export contact csv
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("export-contact-csv")]
        public async Task<ActionResult> ExportContactByCsv()
        {
            var command = new GetContactCsvQuery();
            var fileSupport = await _mediator.Send(command);

            return File(fileSupport.Stream, fileSupport.ContentType, fileSupport.FileName);
        }

        /// <summary>
        /// import contact csv
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("import-contact-csv")]
        public async Task<ActionResult> ImportContactByCsv(IFormFile file)
        {
            var command = new ImportContactCommand() { File = file };

            var resultModel = await _mediator.Send(command);

            if (resultModel.IsSucceeded())
            {
                return Ok();
            }
            return BadRequest(resultModel);
        }
    }
}
