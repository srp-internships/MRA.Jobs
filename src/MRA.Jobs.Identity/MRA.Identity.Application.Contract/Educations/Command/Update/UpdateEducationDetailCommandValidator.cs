using FluentValidation;

namespace MRA.Identity.Application.Contract.Educations.Command.Update;

public class UpdateEducationDetailCommandValidator: AbstractValidator<UpdateEducationDetailCommand>
{
    public UpdateEducationDetailCommandValidator()
    {
        RuleFor(e => e.Id).NotEmpty();
        RuleFor(e => e.University).NotEmpty().MinimumLength(3);
        RuleFor(e => e.Speciality).NotEmpty().MinimumLength(5);
        RuleFor(e => e.StartDate).NotEmpty();
    }
}