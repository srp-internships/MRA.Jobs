using System;
using MediatR;
using MRA.Identity.Application.Contract.Admin.Responses;

namespace MRA.Identity.Application.Contract.User.Commands
{
    public class GoogleAuthCommand: IRequest<ApplicationResponse<JwtTokenResponse>>
    {
        public string Token { get; set; }
    }
}

