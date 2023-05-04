using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Domain.Entities;
public class Application : BaseAuditableEntity
{
    public Applicant Applicant { get; set; }
    public long ApplicantId { get; set; }

    public string CoverLetter { get; set; }

    public string History { get; set; } 

    public Vacancy Vacancy { get; set; }
    public long VacancyId { get; set; }

    public string ResumeUrl { get; set; }

    public ApplicationStatus StatusId { get; set; }
}
