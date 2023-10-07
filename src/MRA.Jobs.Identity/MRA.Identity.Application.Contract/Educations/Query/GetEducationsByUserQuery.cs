using MediatR;
using MRA.Identity.Application.Contract.Educations.Responses;

namespace MRA.Identity.Application.Contract.Educations.Query;

public class GetEducationsByUserQuery : IRequest<ApplicationResponse<List<UserEducationResponse>>
    >
{
    public string? UserName { get; set; }
}