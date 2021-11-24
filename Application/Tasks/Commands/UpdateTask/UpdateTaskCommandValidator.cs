using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CreateTask
{

    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty();
            
            RuleFor(v => v.TaskName)
                .MaximumLength(100);
        }
    }
}
