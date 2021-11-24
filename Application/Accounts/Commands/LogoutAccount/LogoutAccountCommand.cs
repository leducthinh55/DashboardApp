using Application.Common.Models;
using DashboardApp.Application.Common.Exceptions;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Accounts.Commands.LogoutAccount
{
    public class LogoutAccountCommand : IRequest<ResultModel>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LogoutAccountCommandHandler : IRequestHandler<LogoutAccountCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public LogoutAccountCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        /// <summary>
        /// handle login 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(LogoutAccountCommand request, CancellationToken cancellationToken)
        {
            var currentAccountId = _currentUserService.AccountId;
            // find account by username and hash password
            var account = await _context.Accounts.FindAsync(currentAccountId);
            // if account not exist, return not found
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.Username);
            }
            var resultModel = new ResultModel();

            account.RefreshToken = null;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
            resultModel.Succeeded();
            return resultModel;
        }
    }
}
