using Application.Common.Models;
using AutoMapper;
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

namespace Application.Dashboards.Queries.GetDashboardDetail
{
    public class GetDashboardDetailQuery : IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class GetDashboardCommandHandler : IRequestHandler<GetDashboardDetailQuery, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetDashboardCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// handle get Dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(GetDashboardDetailQuery request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var entity = await _context.Dashboards.Where(c => c.Id.Equals(request.Id)).Include(c => c.Widgets).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new NotFoundException(nameof(Dashboard), request.Id);
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
