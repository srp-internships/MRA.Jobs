using FluentValidation;
using MRA.Identity.Application.Contract.User.Commands;
using static MRA.Identity.Application.Contract.IdentityRoles;
namespace MRA.Identity.Application.Features.Applicants.Command.RegisterUser;

public class RegisterUserCommandValidator:AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(s => s.Username.Length > 3);
        RuleFor(s => s.Password.Length > 7);
        RuleFor(s => s.Role == Admin || s.Role == Worker || s.Role ==Intern);
    }
}