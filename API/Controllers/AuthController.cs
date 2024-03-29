﻿using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.LogoutAccount;
using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Queries.GetRefreshToken;
using Application.Accounts.Queries.LoginAccount;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResultModel>> Create(CreateAccountCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateAccountCommand command)
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

        [HttpGet("verification/{id}")]
        public async Task<ActionResult> Verification(Guid id)
        {
            var command = new VerificationAccountCommand()
            {
                Id = id
            };
            var resultModel = await _mediator.Send(command);
            if (resultModel.IsSucceeded())
            {
                return Ok("User is verification");
            }
            return BadRequest(resultModel);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginAccountCommand command)
        {
            var account = await _mediator.Send(command);
            return Ok(account);
        }

        [HttpGet("refresh-token")]
        public async Task<ActionResult> GetRefreshToken(string refreshToken)
        {
            var command = new GetRefreshTokenQuery()
            {
                Token = refreshToken
            };
            var token = await _mediator.Send(command);
            return Ok(token);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var command = new LogoutAccountCommand() { };
            var token = await _mediator.Send(command);
            return Ok(token);
        }
    }
}
