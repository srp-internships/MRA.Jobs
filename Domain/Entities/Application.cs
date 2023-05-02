namespace MRA.Jobs.Domain.Entities;
public class Application : BaseEntity
{
    public Applicant Applicant { get; set; }
    public long ApplicantId { get; set; }
    public Vacancy Vacancy { get; set; }
    public long VacancyId { get; set; }
    public string ApplicantCvPath { get; set; }
    public string ApplicantAbout { get; set; }
    public DateTime ApplicationDate { get; set; }
    public long StatusId { get; set; }
}
