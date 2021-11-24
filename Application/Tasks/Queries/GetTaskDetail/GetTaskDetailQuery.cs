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

namespace Application.Tasks.Queries.GetTaskDetail
{
    public class GetTaskDetailQuery : IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class GetTaskCommandHandler : IRequestHandler<GetTaskDetailQuery, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetTaskCommandHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle get Task
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(GetTaskDetailQuery request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var entity = await _context.Tasks.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(DashboardApp.Domain.Entities.Task), request.Id);
            }
            if (!entity.CreatedBy.Equals(_currentUserService.AccountId))
            {
                resultModel.Failure((int)ProcessStatus.NotPermission);
                return resultModel;
            }
            resultModel.Succeeded(entity);
            return resultModel;
        }
    }
}
