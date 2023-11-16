using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Commands.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(s => s.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$").WithMessage("Password must have at least one lowercase letter, one uppercase letter, one digit, and one special character, with a minimum length of 8 characters.");
        RuleFor(s => s.PhoneNumber).Matches(@"^\+992\d{9}$")
            .WithMessage("Invalid phone number. Example : +992921234567");
        RuleFor(s => s.ConfirmPassword).Equal(s => s.Password).WithMessage("ConfirmPassword must be equal to Password.");
    }
}
