using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(s => s.Email).EmailAddress();
        RuleFor(s=>s.Username).NotEmpty();
        RuleFor(s=>s.FirstName).NotEmpty();
        RuleFor(s=>s.LastName).NotEmpty();
        RuleFor(s => s.PhoneNumber).Matches(@"^(?:\d{9}|\+992\d{9}|992\d{9})$").WithMessage("Invalid phone number. Example : +992921234567, 992921234567, 921234567");
        RuleFor(s => s.Username.Length > 3);
        RuleFor(s => s.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("Password must have at least one alphabetical character, one digit, and be at least 8 characters long.");
        RuleFor(s => !string.IsNullOrEmpty(s.Role));
        RuleFor(s => !string.IsNullOrEmpty(s.Application));
    }
}