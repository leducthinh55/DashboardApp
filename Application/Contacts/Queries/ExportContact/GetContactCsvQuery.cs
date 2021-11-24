using AutoMapper;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using Domain.FileModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts.Queries.ExportContact
{
    public class GetContactCsvQuery : IRequest<FileSupport>
    {
    }

    public class GetContactCsvHandler : IRequestHandler<GetContactCsvQuery, FileSupport>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICsvFileBuilder _csvFileBuilder;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetContactCsvHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService,
            ICsvFileBuilder csvFileBuilder,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _csvFileBuilder = csvFileBuilder;
        }

        /// <summary>
        /// handle get contact
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FileSupport> Handle(GetContactCsvQuery request, CancellationToken cancellationToken)
        {
            var currentAccountId = _currentUserService.AccountId;
            var entites = await _context.Contacts.Where(c => c.CreatedBy.Equals(currentAccountId))
                .Select(c => _mapper.Map<Contact, ContactRecord>(c)).ToListAsync();
            var dataContact = _csvFileBuilder.BuildContactRecordFile(entites);
            var fileSupport = new FileSupport()
            {
                ContentType = "text/csv",
                FileName = "Contact Export",
                Stream = dataContact
            };
            return fileSupport;
        }
    }
}
