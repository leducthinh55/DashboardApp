using Application.Common.Interfaces;
using Application.Common.Models;
using DashboardApp.Application.Common.Exceptions;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Accounts.Queries.LoginAccount
{
    public class LoginAccountCommand : IRequest<ResultModel>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginAccountCommandHandler : IRequestHandler<LoginAccountCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEncryptor _encryptor;
        private readonly IJwtTokenService _jwtTokenService;
        public LoginAccountCommandHandler(IApplicationDbContext context, IEncryptor encryptor, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _encryptor = encryptor;
            _jwtTokenService = jwtTokenService;
        }
        /// <summary>
        /// handle login 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
        {
            // encryp password
            var password = _encryptor.MD5Hash(request.Password);
            // find account by username and hash password
            var account = await _context.Accounts.FirstOrDefaultAsync(_ => _.Username == request.Username && _.Password == password);
            // if account not exist, return not found
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.Username);
            }
            var resultModel = new ResultModel();
            // if account not yet verification, return fail
            if (!account.IsVerificationEmail)
            {
                resultModel.Failure((int)ProcessStatus.NotVerification);
                return resultModel;
            }
            var jwtToken = _jwtTokenService.CreateToken(account);

            var refreshToken = _jwtTokenService.CreateRefreshToken();
            account.RefreshToken = refreshToken;
            account.RevokeTime = DateTime.Now.AddDays(7);
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            var accountDTO = new LoginAccountVM()
            {
                Username = account.Username,
                FullName = account.FullName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };
            resultModel.Succeeded(accountDTO);
            return resultModel;
        }
    }
}
