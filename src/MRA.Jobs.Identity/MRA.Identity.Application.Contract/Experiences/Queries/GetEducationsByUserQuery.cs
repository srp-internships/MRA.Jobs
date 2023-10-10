using MediatR;
using MRA.Identity.Application.Contract.Experiences.Responses;

namespace MRA.Identity.Application.Contract.Experiences.Query;

public class GetExperiencesByUserQuery : IRequest<ApplicationResponse<List<UserExperienceResponse>>
    >
{
    public string? UserName { get; set; }
}