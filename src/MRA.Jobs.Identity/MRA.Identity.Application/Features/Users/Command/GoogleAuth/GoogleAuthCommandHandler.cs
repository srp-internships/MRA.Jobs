using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands.GoogleAuth;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.GoogleAuth;

public class GoogleAuthCommandHandler : IRequestHandler<GoogleAuthCommand, JwtTokenResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGoogleTokenService _tokenService;
    private readonly IJwtTokenService _jwtTokenService;

    private readonly ISender _mediator;

    public GoogleAuthCommandHandler(UserManager<ApplicationUser> userManager, ISender mediator,
        IGoogleTokenService tokenService, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _mediator = mediator;
        _tokenService = tokenService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<JwtTokenResponse> Handle(GoogleAuthCommand request, CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient();
        var tokenEndpoint = "https://accounts.google.com/o/oauth2/token";

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", request.Code },
            { "client_id", "452422541089-q2ilbqdhqs18bbn356qkkbk7bcqihoka.apps.googleusercontent.com" },
            { "client_secret", "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" },
            {
                "scope",
                "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email"
            }
        };

        var response = await httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(parameters));

        var responseContent = await response.Content.ReadAsStringAsync();

        var googlePayload = await _tokenService.VerifyGoogleToken(responseContent);
        if (googlePayload == null)
            throw new ValidationException("Invalid google token");

        var info = new UserLoginInfo("google", googlePayload.Subject, "google");
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
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

                var registerCommandResult = await _mediator.Send(registerCommand, cancellationToken);
                user = await _userManager.FindByIdAsync(registerCommandResult.ToString());
            }
        }

        if (user == null) throw new ValidationException("User not found");

        var claims = await _userManager.GetClaimsAsync(user);

        return new JwtTokenResponse
        {
            AccessToken = _jwtTokenService.CreateTokenByClaims(claims, out var exp),
            RefreshToken = _jwtTokenService.CreateRefreshToken(claims),
            AccessTokenValidateTo = exp
        };
    }


    public string RandomPassword()
    {
        string[] randomChars = new[]
        {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ", "abcdefghijkmnopqrstuvwxyz", "0123456789", "!@$?_-"
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