namespace MRA.Jobs.Domain.Entities;

public class VacancyTag : BaseEntity
{
    public Tag Tag { get; set; }
    public Guid TagId { get; set; }
    public Vacancy Vacancy { get; set; }
    public Guid VacancyId { get; set; }
}