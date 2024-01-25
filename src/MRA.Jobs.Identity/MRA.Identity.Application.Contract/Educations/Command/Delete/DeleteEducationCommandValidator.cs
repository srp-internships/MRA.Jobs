using FluentValidation;

namespace MRA.Identity.Application.Contract.Educations.Command.Delete;

public class DeleteEducationCommandValidator : AbstractValidator<DeleteEducationCommand>
{
    public DeleteEducationCommandValidator()
    {
        RuleFor(e=>e.Id).NotEmpty();
    }
}