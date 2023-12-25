using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.Dtos.Responses;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

public class InternshipVacancyListResponse
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime ApplicationDeadline { get; set; }

    public int Duration { get; set; }
    public int Stipend { get; set; }
    public string Slug { get; set; }
    public IEnumerable<VacancyQuestionDto> VacancyQuestions { get; set; }
    public IEnumerable<VacancyTaskResponseDto> VacancyTasks { get; set; }
}
