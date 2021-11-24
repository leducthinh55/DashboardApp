using Application.Common.Models;
using DashboardApp.Application.Common.Exceptions;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Accounts.Commands.UpdateAccount
{
    public class VerificationAccountCommand : IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class VerificationAccountCommandHandler : IRequestHandler<VerificationAccountCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;

        public VerificationAccountCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// handle verification account
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(VerificationAccountCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            // find account in database
            var account = await _context.Accounts.FindAsync(request.Id);
            // if account is null, throw not found exception
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.Id);
            }
            // if account is not null, change property IsVerificationEmail to true
            account.IsVerificationEmail = true;
            _context.Accounts.Update(account);
            int result = await _context.SaveChangesAsync(cancellationToken);
            // update successful
            if (result > 0)
            {
                resultModel.Succeeded();
                return resultModel;                
            }
            resultModel.Failure((int)ProcessStatus.Fail);
            return resultModel;
        }
    }
}
