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
        RuleFor(s => s.Username.Trim()).MinimumLength(4)
            .Must(username => username.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '@'))
            .WithMessage("Username should only consist of letters, numbers, underscores, or the @ symbol.");
        
        RuleFor(s => s.Password).NotEmpty().MinimumLength(5);
        RuleFor(s => !string.IsNullOrEmpty(s.Role));
        RuleFor(s => !string.IsNullOrEmpty(s.Application));
        RuleFor(s => s.ConfirmPassword).Equal(s => s.Password).WithMessage("Passwords do not match.");
    }
}