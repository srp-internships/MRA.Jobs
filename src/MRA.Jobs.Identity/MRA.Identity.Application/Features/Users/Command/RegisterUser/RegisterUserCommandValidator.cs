using FluentValidation;
using MRA.Identity.Application.Contract.User.Commands;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(s => s.Username.Length > 3);
        RuleFor(s => s.Password.Length > 7);
        RuleFor(user => user.ConfirmPassword).Equal(user => user.Password).WithMessage("Passwords must match.");
    }
}