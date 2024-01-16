using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Commands.ChangePassword;
public class ChangePasswordUserCommandValidator : AbstractValidator<ChangePasswordUserCommand>
{
    public ChangePasswordUserCommandValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(5);
        RuleFor(x => x.ConfirmPassword).NotEmpty()
    .Equal(x => x.NewPassword)
    .WithMessage("Passwords do not math.");

    }
}
