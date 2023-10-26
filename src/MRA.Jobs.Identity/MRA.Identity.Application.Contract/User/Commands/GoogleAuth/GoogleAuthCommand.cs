using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Commands.GoogleAuth
{
    public class GoogleAuthCommand: IRequest<JwtTokenResponse>
    {
        public string Code { get; set; } = null!;
    }
}

