using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Commands.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(s => s.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("Password must have at least one alphabetical character, one digit, and be at least 8 characters long.");
        RuleFor(s => s.PhoneNumber).Matches(@"^\+992\d{9}$")
            .WithMessage("Invalid phone number. Example : +992921234567");
        RuleFor(s => s.ConfirmPassword).Equal(s => s.Password).WithMessage("ConfirmPassword must be equal to Password.");
    }
}
