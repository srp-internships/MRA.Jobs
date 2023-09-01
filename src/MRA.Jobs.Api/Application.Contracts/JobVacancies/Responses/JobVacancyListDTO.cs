using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Application.Contracts.Converter.Converter;
using Newtonsoft.Json;
using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;

public class JobVacancyListDto
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

    public Dtos.Enums.ApplicationStatusDto.WorkSchedule WorkSchedule { get; set; }
    public ICollection<TimeLineDetailsDto> History { get; set; }
    public ICollection<TagDto> Tags { get; set; }
    public IEnumerable<VacancyQuestionDto> VacancyQuestions { get; set; }
    public string Slug { get; set; }
}