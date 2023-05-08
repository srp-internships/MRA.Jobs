namespace MRA.Jobs.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public ICollection<VacancyTag> VacancyTags { get; set; }
    public ICollection<UserTag> UserTags { get; set; }
}
