using Application.Common.Models;
using AutoMapper;
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

namespace Application.Dashboards.Commands.CreateDashboard
{
    public class CreateDashboardCommand : DashboardDTO, IRequest<ResultModel>
    {
        public ICollection<CreateWidget> Widgets { get; set; }
    }

    public class CreateWidget : WidgetDTO
    {
        
    }

    public class CreateDashboardCommandHandler : IRequestHandler<CreateDashboardCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateDashboardCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// handle create Dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(CreateDashboardCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            // mapping CreateDashboardCommand to Dashboard
            var entity = _mapper.Map<CreateDashboardCommand, Dashboard>(request);

            await _context.Dashboards.AddAsync(entity);
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
