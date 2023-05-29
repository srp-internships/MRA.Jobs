namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class CreateApplicationCommand:IRequest<Guid>
{
    public Guid ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public string CV { get; set; }
}
