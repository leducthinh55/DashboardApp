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

namespace Application.Contacts.Queries.GetListContact
{
    public class GetListContactQuery : IRequest<ResultModel>
    {
        public string Keyword { get; set; }
    }

    public class GetListContactQueryHandler : IRequestHandler<GetListContactQuery, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetListContactQueryHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// handle get contacts
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(GetListContactQuery request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var query = _context.Contacts.Where(_ =>
                _.CreatedBy.Equals(_currentUserService.AccountId));
            var keyword = request.Keyword;
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(_ => _.FirstName.ToLower().Contains(keyword)
                || _.LastName.ToLower().Contains(keyword)
                || _.Title.ToLower().Contains(keyword)
                || _.Department.ToLower().Contains(keyword));
            }
            var entities = await query.ToListAsync();
            resultModel.Succeeded(entities);
            return resultModel;
        }
    }
}
