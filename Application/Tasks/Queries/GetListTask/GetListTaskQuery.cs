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

namespace Application.Tasks.Queries.GetListTask
{
    public class GetListTaskQuery : IRequest<ResultModel>
    {
        public string Keyword { get; set; }
    }

    public class GetListTaskQueryHandler : IRequestHandler<GetListTaskQuery, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetListTaskQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle get Tasks
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(GetListTaskQuery request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var query = _context.Tasks.Where(_ =>
                _.CreatedBy.Equals(_currentUserService.AccountId));
            var keyword = request.Keyword;
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(_ => _.TaskName.ToLower().Contains(keyword));
            }
            var entities = await query.ToListAsync();
            resultModel.Succeeded(entities);
            return resultModel;
        }
    }
}
