using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Entities;
using Domain.Enums;
using Domain.MailModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : AccountDTO, IRequest<ResultModel>
    {
        public string Username { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ResultModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMailSender _mailSender;
        private readonly IEncryptor _encryptor;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(IApplicationDbContext context, IEncryptor encryptor, IMapper mapper, IMailSender mailSender)
        {
            _context = context;
            _encryptor = encryptor;
            _mapper = mapper;
            _mailSender = mailSender;
        }

        /// <summary>
        /// handle create account
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var resultModel = new ResultModel();
            // encryp password
            var password = _encryptor.MD5Hash(request.Password);
            var entity = _mapper.Map<CreateAccountCommand, Account>(request);
            entity.Password = password;

            try
            {
                await _context.Accounts.AddAsync(entity);
                int result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    resultModel.Failure((int)ProcessStatus.Fail);
                }
            }
            catch (Exception e)
            {
                //Duplicate
                if (e is DbUpdateException dbUpdateEx)
                {
                    resultModel.Failure((int)ProcessStatus.Duplicated);
                    return resultModel;
                }
            }

            resultModel.Succeeded(entity.Id);
            // send mail to verification
            var message = new Message()
            {
                To = entity.Email,
                Subject = "Verification Email DashboardApp",
                Content = $"https://localhost:5001/api/Auth/verification/{entity.Id}",
            };

            await _mailSender.SendEmailAsync(message);
            return resultModel;
        }
    }
}
