using Application.Common.Models;
using AutoMapper;
using DashboardApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetReportTaskProgress
{
    public class GetReportTaskProgressQuery : IRequest<ResultModel>
    {
    }
    public class GetReportTaskProgressQueryHandler : IRequestHandler<GetReportTaskProgressQuery, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetReportTaskProgressQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModel> Handle(GetReportTaskProgressQuery request, CancellationToken cancellationToken)
        {
            var currentAccountId = _currentUserService.AccountId;
            // query to get task by current user
            var query = _context.Tasks.Where(c => c.CreatedBy.Equals(currentAccountId));
            // get number of total task
            var numsTask = await query.CountAsync();
            // get number of total task complete
            var numsTaskCompleted = await query.CountAsync(_ => _.IsCompleted);

            // number task incomplete equals number total task subtraction completed task
            var report = new ReportTaskProgressVM() {
                Total = numsTask,
                Completed = numsTaskCompleted,
                InCompleted = numsTask - numsTaskCompleted
            };
            var resultModel = new ResultModel();
            resultModel.Succeeded(report);
            return resultModel;
        }
    }
}
