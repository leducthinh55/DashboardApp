using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contacts.Commands.CreateContact
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .MaximumLength(100);
            
            RuleFor(v => v.LastName)
                .MaximumLength(100);

            RuleFor(v => v.Title)
                .MaximumLength(50);

            RuleFor(v => v.Department)
                .MaximumLength(50);
        }
    }
}
