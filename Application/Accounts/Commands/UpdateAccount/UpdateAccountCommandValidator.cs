using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
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
