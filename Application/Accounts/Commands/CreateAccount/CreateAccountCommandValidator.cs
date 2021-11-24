using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(v => v.Username)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(v => v.Password)
                .MaximumLength(50)
                .MinimumLength(6);                

            RuleFor(v => v.Email)
                .EmailAddress();

            RuleFor(v => v.FullName)
                .MaximumLength(200);
        }
    }
}
