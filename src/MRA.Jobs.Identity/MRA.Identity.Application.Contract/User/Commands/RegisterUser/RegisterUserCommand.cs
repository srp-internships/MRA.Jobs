﻿using MediatR;

namespace MRA.Identity.Application.Contract.User.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<ApplicationResponse<Guid>>
{
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string PhoneNumber { get; set; } = "";

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; }
}