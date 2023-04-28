namespace MRA_Jobs.Domain.Entities;
public class Application: BaseEntity
{
    public Applicant Applicant { get; set; }
    public int ApplicantId { get; set; }
    public Vacancy Vacancy { get; set; }
    public int VacancyId { get; set; }
    public string ApplicantCvPath { get; set; }
    public string ApplicantAbout { get; set; }
    public DateTime ApplicationDate { get; set; }
    public Status Status { get; set; }
    public int StatusId { get; set; }
}
