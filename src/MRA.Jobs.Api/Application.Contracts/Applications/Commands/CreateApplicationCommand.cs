using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Contracts.Applications.Commands;

public class CreateApplicationCommand : IRequest<Guid>
{
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public IEnumerable<JobQuestionDto> JobQuestions { get; set; }
}