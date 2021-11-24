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

namespace Application.Dashboards.Commands.DeleteDashboard
{
    public class DeleteDashboardCommand : IRequest<ResultModel>
    {
        public Guid Id { get; set; }
    }

    public class DeleteDashboardCommandHandler : IRequestHandler<DeleteDashboardCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DeleteDashboardCommandHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle create Dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(DeleteDashboardCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var entity = await _context.Dashboards.Where(d => d.Id.Equals(request.Id)).Include(d => d.Widgets).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new NotFoundException(nameof(Dashboard), request.Id);
            }
            if (!entity.CreatedBy.Equals(_currentUserService.AccountId))
            {
                resultModel.Failure((int)ProcessStatus.NotPermission);
                return resultModel;
            }
            var widgets = entity.Widgets;
            if (widgets.Any())
            {
                _context.Widgets.RemoveRange(entity.Widgets);
            }
            _context.Dashboards.Remove(entity);
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
