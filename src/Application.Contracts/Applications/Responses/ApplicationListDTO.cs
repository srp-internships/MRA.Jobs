using MRA.Jobs.Application.Common.Converter;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.Applications.Responses;
public class ApplicationListDTO
{
    public Guid ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public string History { get; set; }
    public Guid VacancyId { get; set; }
    public string ResumeUrl { get; set; }
    public int StatusId { get; set; }
}

public class ApplicationDetailsDTO
{
    public Guid ApplicantId { get; set; }
    public string CoverLetter { get; set; }
    public string History { get; set; }
    public Guid VacancyId { get; set; }
    public string ResumeUrl { get; set; }
    public int StatusId { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}