using MediatR;

namespace MRA.Identity.Application.Contract.Experiences.Commands;
public class CreateExperienceDetailCommand : IRequest<ApplicationResponse<Guid>>
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsCurrentJob { get; set; }
    public string Description { get; set; }
    public string Address { get; set; } 
}
