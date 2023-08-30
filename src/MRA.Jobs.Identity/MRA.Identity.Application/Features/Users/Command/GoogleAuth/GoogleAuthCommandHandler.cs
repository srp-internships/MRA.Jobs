using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Admin.Responses;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Services;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.GoogleAuth;

public class GoogleAuthCommandHandler : IRequestHandler<GoogleAuthCommand, ApplicationResponse<JwtTokenResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGoogleTokenService _tokenService;
    private readonly ISender _mediatR;

    public GoogleAuthCommandHandler(UserManager<ApplicationUser> userManager, ISender mediatR,IGoogleTokenService tokenService)
    {
        _userManager = userManager;
        _mediatR = mediatR;
        _tokenService = tokenService;
    }

    public async Task<ApplicationResponse<JwtTokenResponse>> Handle(GoogleAuthCommand request, CancellationToken cancellationToken)
    {
        var googlePayload = await _tokenService.VerifyGoogleToken(request);
        if (googlePayload == null)
            return null;

        var info = new UserLoginInfo("google", googlePayload.Subject, "google");
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        ApplicationResponse<Guid>? userResponseModel = null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(googlePayload.Email);
            if (user == null)
            {
                var registerCommand = new RegisterUserCommand
                {
                    Email = googlePayload.Email,
                    FirstName = googlePayload.Name,
                    Username = googlePayload.Email,
                    Password = RandomPassword()

                };

                var registerCommandResult = await _mediatR.Send(registerCommand);
                if (registerCommandResult.IsSuccess)
                {
                    user = await _userManager.FindByIdAsync(registerCommandResult.Response.ToString());
                }
                else
                {
                    return new ApplicationResponseBuilder<JwtTokenResponse>().Success(false).SetErrorMessage("register fail").Build();
                }
            }
            await _userManager.AddLoginAsync(user, info);
        }
        return new ApplicationResponseBuilder<JwtTokenResponse>().SetResponse(await _tokenService.GenerateTokens(user));
    }


    public string RandomPassword()
    {
        string[] randomChars = new[]
        {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-"
            };

        Random rand = new Random(Environment.TickCount);
        List<char> chars = new List<char>();

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[0][rand.Next(0, randomChars[0].Length)]);

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[1][rand.Next(0, randomChars[1].Length)]);

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[2][rand.Next(0, randomChars[2].Length)]);

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[3][rand.Next(0, randomChars[3].Length)]);

        for (int i = chars.Count; i < 8; i++)
        {
            string rcs = randomChars[rand.Next(0, randomChars.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}



