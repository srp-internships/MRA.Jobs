using MRA.Jobs.Application.Contracts.Dtos.Responses;

namespace MRA.Jobs.Application.Contracts.NoVacancies.Responses;

public class NoVacancyResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public IEnumerable<VacancyQuestionResponseDto> VacancyQuestions { get; set; }
}