using System.ComponentModel.DataAnnotations;

namespace MRA.Jobs.Domain.Entities;

public class VacancyCategory : BaseEntity
{
    public string Name { get; set; }
    [Key]
    public string Slug { get; set; }
    public ICollection<Vacancy> Vacancies { get; set; }
}
