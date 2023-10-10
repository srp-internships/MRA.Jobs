using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MRA.Identity.Application.Contract.Educations.Command.Create;
public class CreateEducationDetailCommandValidator : AbstractValidator<CreateEducationDetailCommand>
{
    public CreateEducationDetailCommandValidator()
    {
        RuleFor(e => e.University).NotEmpty().MinimumLength(3);
        RuleFor(e => e.Speciality).NotEmpty().MinimumLength(5);
        RuleFor(e => e.StartDate).NotEmpty();
    }
}
