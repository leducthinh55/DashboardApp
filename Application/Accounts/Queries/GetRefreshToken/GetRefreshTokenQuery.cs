using Application.Common.Interfaces;
using Application.Common.Models;
using DashboardApp.Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Accounts.Queries.GetRefreshToken
{
    public class GetRefreshTokenQuery : IRequest<ResultModel>
    {
        public string Token { get; set; }
    }

    public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        public GetRefreshTokenQueryHandler(IApplicationDbContext context,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }
        /// <summary>
        /// handle refresh token 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var refreshToken = request.Token;
            var account = await _context.Accounts.Where(a => a.RefreshToken == refreshToken).FirstOrDefaultAsync();
            // if account is null or refresh token is expire, return not permission
            if (account == null || account.RevokeTime < DateTime.Now)
            {
                resultModel.Failure((int)ProcessStatus.NotPermission);
                return resultModel;
            }

            // get new fresh token
            var newRefreshToken = _jwtTokenService.CreateRefreshToken();
            // update new refresh token
            account.RefreshToken = newRefreshToken;
            account.RevokeTime = DateTime.Now.AddDays(7);

            _context.Accounts.Update(account);

            await _context.SaveChangesAsync(cancellationToken);
            var token = _jwtTokenService.CreateToken(account);
            resultModel.Succeeded(new { token = token, refreshToken = newRefreshToken });

            return resultModel;
        }
    }
}
