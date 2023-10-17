using MediatR;

namespace MRA.Identity.Application.Contract.Educations.Command.Create;
public class CreateEducationDetailCommand: IRequest<ApplicationResponse<Guid>>
{
    public string University { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool UntilNow { get; set; }
    public string Speciality { get; set; }
}
