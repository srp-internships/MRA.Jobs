using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Commands.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(s => s.Password.Length > 7);
        RuleFor(s => s.PhoneNumber).Matches(@"^\+992\d{9}$")
            .WithMessage("Invalid phone number. Example : +992921234567");
    }
}
