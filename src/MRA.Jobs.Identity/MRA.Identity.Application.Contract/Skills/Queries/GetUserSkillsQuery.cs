using MediatR;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Identity.Application.Contract.Skills.Queries;
public class GetUserSkillsQuery : IRequest<UserSkillsResponse>
{
    public string? UserName { get; set; }
}
