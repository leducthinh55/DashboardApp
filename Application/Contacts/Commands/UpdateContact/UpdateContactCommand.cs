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

namespace Application.Contacts.Commands.CreateContact
{
    public class UpdateContactCommand : ContactDTO, IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateContactCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle update contact
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
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
            // update property
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Title = request.Title;
            entity.Department = request.Department;

            _context.Contacts.Update(entity);
            int result = await _context.SaveChangesAsync(cancellationToken);

            resultModel.Succeeded(entity.Id);
            return resultModel;
        }
    }
}
