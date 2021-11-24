using Application.Common.Models;
using AutoMapper;
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

namespace Application.Contacts.Commands.DeleteContact
{
    public class DeleteContactCommand : IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DeleteContactCommandHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle create contact
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var entity = await _context.Contacts.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Contact), request.Id);
            }
            if (!entity.CreatedBy.Equals(_currentUserService.AccountId))
            {
                resultModel.Failure((int)ProcessStatus.NotPermission);
                return resultModel;
            }
            _context.Contacts.Remove(entity);
            int result = await _context.SaveChangesAsync(cancellationToken);
            // if no change in database
            if (result == 0)
            {
                resultModel.Failure((int)ProcessStatus.Fail);
                return resultModel;
            }
            resultModel.Succeeded(entity.Id);
            return resultModel;
        }
    }
}
