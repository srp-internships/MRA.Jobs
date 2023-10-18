using MediatR;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Identity.Application.Contract.Skills.Command;
public class RemoveUserSkillCommand : IRequest<UserSkillsResponse>
{
    public string Skill {  get; set; }
}
