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

namespace Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : AccountDTO, IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class UpdateAccountCommandHandle : IRequestHandler<UpdateAccountCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAccountCommandHandle(IApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// handle update account
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            // find account in database
            var entity = await _context.Accounts.FindAsync(request.Id);
            // if account is null, throw not found exception
            if (entity == null)
            {
                throw new NotFoundException(nameof(Account), request.Id);
            }
            // update property of account
            entity.Password = request.Password;
            entity.FullName = request.FullName;
            entity.Email = request.Email;

            await _context.SaveChangesAsync(cancellationToken);
            resultModel.Succeeded();
            return resultModel;
        }
    }
}
