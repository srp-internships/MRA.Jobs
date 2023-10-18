﻿using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;

namespace MRA.Jobs.Client.Services.Auth;

public interface IAuthService
{
    Task<string> RegisterUserAsync(RegisterUserCommand command);
    Task<string> LoginUserAsync(LoginUserCommand command);
}
