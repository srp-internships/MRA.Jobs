using MediatR;
using MRA.Identity.Application.Contract.Application.Responses;

namespace MRA.Identity.Application.Contract.Application.Commands;

public class CreateApplicationCommand:IRequest<ApplicationResponse<JwtTokenResponse>>
{
    public string ApplicationName { get; set; } = "";
    public string? ByDefaultRole { get; set; }
}