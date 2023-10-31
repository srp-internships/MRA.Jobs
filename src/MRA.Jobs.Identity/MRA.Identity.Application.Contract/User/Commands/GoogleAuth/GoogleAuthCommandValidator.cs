using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Commands.GoogleAuth;

public class GoogleAuthCommandValidator:AbstractValidator<GoogleAuthCommand>
{
    public GoogleAuthCommandValidator()
    {
        RuleFor(s => s.Code).NotNull().NotEmpty();
    }
}