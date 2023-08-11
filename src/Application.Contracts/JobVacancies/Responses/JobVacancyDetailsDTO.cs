using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;

public class JobVacancyDetailsDto
{
    public Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }

    public WorkSchedule WorkSchedule { get; set; }
    public int RequiredYearOfExperience { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? LastModifiedAt { get; set; }

    public Guid? LastModifiedBy { get; set; }
    public ICollection<TimeLineDetailsDto> History { get; set; }
    public ICollection<TagDto> Tags { get; set; }
}