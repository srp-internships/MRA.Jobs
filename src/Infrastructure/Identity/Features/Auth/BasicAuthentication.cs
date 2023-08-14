﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Identity.Entities;
using MRA.Jobs.Infrastructure.Identity.Services;
using MRA.Jobs.Infrastructure.Shared.Auth.Commands;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.Infrastructure.Identity.Features.Auth;

public class BasicAuthenticationCommandValidator : AbstractValidator<BasicAuthenticationCommand>
{
    public BasicAuthenticationCommandValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}

public class BasicAuthenticationHandler : IRequestHandler<BasicAuthenticationCommand, JwtTokenResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;

    public BasicAuthenticationHandler(UserManager<ApplicationUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<JwtTokenResponse> Handle(BasicAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var token = await _tokenService.GenerateTokens(user, cancellationToken);
            return token;
        }

        return null;
    }
}
