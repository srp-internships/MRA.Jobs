namespace MRA.Jobs.Domain.Entities;

public class VacancyCategory : BaseEntity
{
    public string Name { get; set; }
    public ICollection<JobVacancy> JobVacancies { get; set; }
    public ICollection<EducationVacancy> EducationVacancies { get; set; }
}
