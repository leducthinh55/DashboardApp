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

namespace Application.Contacts.Commands.CreateContact
{
    public class CreateContactCommand : ContactDTO, IRequest<ResultModel>
    {
    }

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateContactCommandHandler(IApplicationDbContext context, IMapper mapper)
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
        public async Task<ResultModel> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            // mapping CreateContactCommand to Contact
            var entity = _mapper.Map<CreateContactCommand, Contact>(request);

            await _context.Contacts.AddAsync(entity);
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
