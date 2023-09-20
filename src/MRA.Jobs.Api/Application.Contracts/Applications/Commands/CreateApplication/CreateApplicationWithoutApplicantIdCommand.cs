namespace MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
public class CreateApplicationWithoutApplicantIdCommand : IRequest<Guid>
{
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
}