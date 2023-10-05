using MediatR;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Identity.Application.Contract.Skills.Queries;
public class GetUserSkillsQuery : IRequest<ApplicationResponse<UserSkillsResponse>>
{
    public string? UserName { get; set; }
}
