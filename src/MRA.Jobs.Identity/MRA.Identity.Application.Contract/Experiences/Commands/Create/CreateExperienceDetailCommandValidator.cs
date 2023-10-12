using FluentValidation;

namespace MRA.Identity.Application.Contract.Experiences.Commands.Create;

public class CreateExperienceDetailCommandValidator : AbstractValidator<CreateExperienceDetailCommand>
{
    public CreateExperienceDetailCommandValidator()
    {
        RuleFor(e => e.Address).NotEmpty();
        RuleFor(e => e.Description).NotEmpty();
        RuleFor(e => e.CompanyName).NotEmpty();
        RuleFor(e => e.StartDate).NotEmpty();
    }
}
