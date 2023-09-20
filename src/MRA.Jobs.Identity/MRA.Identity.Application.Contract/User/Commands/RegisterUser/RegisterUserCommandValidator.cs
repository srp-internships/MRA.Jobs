﻿using FluentValidation;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;

namespace MRA.Identity.Application.Contract.User.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(s => s.Email).EmailAddress();
        RuleFor(s=>s.Username).NotEmpty();
        RuleFor(s=>s.FirstName).NotEmpty();
        RuleFor(s=>s.LastName).NotEmpty();
        RuleFor(s => s.PhoneNumber).Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number.");
        RuleFor(s => s.Username.Length > 3);
        RuleFor(s => s.Password.Length > 7);
        RuleFor(user => user.ConfirmPassword).Equal(user => user.Password).WithMessage("Passwords must match.");
    }
}