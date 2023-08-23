using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.Applications.Commands;

public class CreateApplicationCommand : IRequest<Guid>
{
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public IEnumerable<JobQuestion> JobQuestions { get; set; }
}