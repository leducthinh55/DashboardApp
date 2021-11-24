using Application.Common.Models;
using AutoMapper;
using DashboardApp.Application.Common.Exceptions;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Dashboards.Commands.CreateDashboard
{
    public class UpdateDashboardCommand : DashboardDTO, IRequest<ResultModel>
    {
        public Guid Id { get; set; }

        public List<UpdateWidget> Widgets { get; set; }
    }

    public class UpdateWidget : WidgetDTO
    {
        public Guid Id { get; set; }

        public Guid DashboardId { get; set; }
    }

    public class UpdateDashboardCommandHandler : IRequestHandler<UpdateDashboardCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateDashboardCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle update Dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(UpdateDashboardCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var entity = await _context.Dashboards.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Dashboard), request.Id);
            }
            if (!entity.CreatedBy.Equals(_currentUserService.AccountId))
            {
                resultModel.Failure((int)ProcessStatus.NotPermission);
                return resultModel;
            }
            // update property
            entity.LayoutType = request.LayoutType;
            entity.Title = request.Title;
            _context.Dashboards.Update(entity);

            // get list widget current in database
            var widgetInDb = await _context.Widgets.Where(c => c.DashBoardId.Equals(entity.Id)).ToListAsync();
            // remove list widget current
            _context.Widgets.RemoveRange(widgetInDb);

            var widgetUpdate = _mapper.Map<List<UpdateWidget>, List<Widget>>(request.Widgets);
            // add new list widget to database
            _context.Widgets.AddRange(widgetUpdate);

            int result = await _context.SaveChangesAsync(cancellationToken);
            if(result == 0)
            {
                resultModel.Failure((int)ProcessStatus.Fail);
            }
            resultModel.Succeeded(entity.Id);
            return resultModel;
        }
    }
}
