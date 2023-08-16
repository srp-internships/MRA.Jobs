using System.ComponentModel.DataAnnotations;

namespace MRA.Jobs.Domain.Entities;

[Index(nameof(Slug), IsUnique = true)]
public class VacancyCategory : BaseEntity
{
    public string Name { get; set; }
    [Key]
    public string Slug { get; set; }
    public ICollection<Vacancy> Vacancies { get; set; }
}
