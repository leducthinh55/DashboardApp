using Application.Common.Models;
using AutoMapper;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contacts.Commands.CreateContact
{
    public class ImportContactCommand : IRequest<ResultModel>
    {
        public IFormFile File { get; set; }
    }

    public class ImportContactCommandHandler : IRequestHandler<ImportContactCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ImportContactCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// handle create contact
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(ImportContactCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            var file = request.File;
            var listContacts = new List<Contact>();
            if (file.FileName.EndsWith(".csv"))
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    string[] headers = reader.ReadLine().Split(',');
                    while (!reader.EndOfStream)
                    {
                        string[] rows = reader.ReadLine().Split(',');
                        var contact = new Contact()
                        {
                            FirstName = rows[0].ToString(),
                            LastName = rows[1].ToString(),
                            Title = rows[2].ToString(),
                            Department = rows[3].ToString()
                        };
                        listContacts.Add(contact);
                    }
                }
            }
            await _context.Contacts.AddRangeAsync(listContacts);
            int result = await _context.SaveChangesAsync(cancellationToken);
            // if no change in database
            if (result == 0)
            {
                resultModel.Failure((int)ProcessStatus.Fail);
                return resultModel;
            }
            resultModel.Succeeded();
            return resultModel;
        }
    }
}
