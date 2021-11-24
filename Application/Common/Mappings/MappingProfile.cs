using Application.Accounts.Commands.CreateAccount;
using Application.Contacts.Commands.CreateContact;
using Application.Contacts.Queries.ExportContact;
using Application.Dashboards.Commands.CreateDashboard;
using Application.Tasks.Commands.CreateTask;
using AutoMapper;
using DashboardApp.Domain.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAccountCommand, Account>();

            CreateMap<CreateContactCommand, Contact>();

            CreateMap<CreateTaskCommand, Task>();

            CreateMap<CreateWidget, Widget>();

            CreateMap<UpdateWidget, Widget>();

            CreateMap<CreateDashboardCommand, Dashboard>();

            CreateMap<Contact, ContactRecord>();
        }
    }
}
