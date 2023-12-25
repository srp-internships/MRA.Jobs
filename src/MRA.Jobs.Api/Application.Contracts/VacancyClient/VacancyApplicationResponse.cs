using MRA.Jobs.Application.Contracts.Dtos.Responses;

namespace MRA.Jobs.Application.Contracts.VacancyClient;
public class VacancyApplicationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Duration { get; set; }
    public int Stipend { get; set; }
    public DateTime Deadline { get; set; }
    public int Fees { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public IEnumerable<VacancyQuestionResponseDto> VacancyQuestions { get; set; }
    public IEnumerable<VacancyTaskResponseDto> VacancyTasks { get; set; }
}
