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

    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}