using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;

namespace MRA.Identity.Application.Contract.User.Commands.LoginUser;
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(s => s.Username).NotEmpty();
        RuleFor(s => s.Password).NotEmpty();
    }
}
