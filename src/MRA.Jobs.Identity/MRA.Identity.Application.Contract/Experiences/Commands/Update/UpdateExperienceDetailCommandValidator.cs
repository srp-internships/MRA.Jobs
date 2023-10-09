using FluentValidation;

namespace MRA.Identity.Application.Contract.Experiences.Commands.Update;
public class UpdateExperienceDetailCommandValidator : AbstractValidator<UpdateExperienceDetailCommand>
{
    public UpdateExperienceDetailCommandValidator()
    {
        RuleFor(e => e.Id).NotEmpty();
        RuleFor(e => e.Address).NotEmpty();
        RuleFor(e => e.Description).NotEmpty();
        RuleFor(e => e.CompanyName).NotEmpty();
        RuleFor(e => e.StartDate).NotEmpty();
    }
}
