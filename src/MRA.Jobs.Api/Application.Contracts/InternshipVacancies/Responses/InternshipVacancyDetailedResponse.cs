using System.Text.Json.Serialization;
using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.Dtos.Responses;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
public record InternshipVacancyDetailedResponse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }

    public int Duration { get; set; }
    public int Fees { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? LastModifiedAt { get; set; }

    public Guid? LastModifiedBy { get; set; }
    public ICollection<TimeLineDetailsDto> History { get; set; }
    public ICollection<string> Tags { get; set; }
    public IEnumerable<VacancyQuestionDto> VacancyQuestions { get; set; }
    public IEnumerable<VacancyTaskResponseDto> VacancyTasks { get; set; }
    public string Slug { get; set; }
}
