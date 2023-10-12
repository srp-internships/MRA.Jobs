using MediatR;
using MRA.Identity.Application.Contract.Skills.Responses;

namespace MRA.Identity.Application.Contract.Skills.Command;
public class AddSkillCommand : IRequest<ApplicationResponse<UserSkillsResponse>>
{
    public List<string> Skills { get; set; }
}
