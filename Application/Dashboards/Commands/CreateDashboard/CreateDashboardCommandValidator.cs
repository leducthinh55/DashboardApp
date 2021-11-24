using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboards.Commands.CreateDashboard
{

    public class CreateDashboardCommandValidator : AbstractValidator<CreateDashboardCommand>
    {
        public CreateDashboardCommandValidator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(100);

            RuleFor(v => v.LayoutType)
                .MaximumLength(50);
        }
    }

    public class CreateWidgetCommandValidator : AbstractValidator<CreateWidget>
    {
        public CreateWidgetCommandValidator()
        {
            RuleFor(v => v.Title)
                .MaximumLength(100);

            RuleFor(v => v.MinHeight)
                .GreaterThan(0);

            RuleFor(v => v.MinWidth)
                .GreaterThan(0);

            RuleFor(v => v.Title)
                .MaximumLength(100);

            RuleFor(v => v.WidgetType)
                .MaximumLength(50);
        }
    }
}
