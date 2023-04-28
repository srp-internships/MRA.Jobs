namespace MRA_Jobs.Domain.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; }
    public ICollection<JobVacancy> JobVacancies { get; set; }
    public ICollection<EducationVacancy> EducationVacancies { get; set; }
}
