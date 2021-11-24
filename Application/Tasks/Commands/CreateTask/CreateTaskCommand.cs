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

namespace Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : TaskDTO, IRequest<ResultModel>
    {
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// handle create Task
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            // mapping CreateTaskCommand to Task
            var entity = _mapper.Map<CreateTaskCommand, DashboardApp.Domain.Entities.Task>(request);

            await _context.Tasks.AddAsync(entity);
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
