namespace MRA.Jobs.Domain.Entities;

public class VacancyCategory : BaseEntity
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public ICollection<Vacancy> Vacancies { get; set; }
}
