using FluentValidation;

namespace MRA.Identity.Application.Contract.Experience.Command.Delete;

public class DeleteExperienceCommandValidator : AbstractValidator<DeleteExperienceCommand>
{
    public DeleteExperienceCommandValidator()
    {
        RuleFor(e=>e.Id).NotEmpty();
    }
}