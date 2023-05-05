using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MRA.Jobs.Domain.Entities;

public abstract class BaseEntity
{
    public long Id { get; set; }


    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}

public class VacancyCategory : BaseEntity
{
    public string Name { get; set; }
    public ICollection<JobVacancy> JobVacancies { get; set; }
    public ICollection<EducationVacancy> EducationVacancies { get; set; }
}

public abstract class Vacancy : BaseAuditableEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public long? CategoryId { get; set; }
    public VacancyCategory Category { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<VacancyTimelineEvent> VacancyTimelineEvents { get; set; }

    public ICollection<VacancyTag> VacancyTags { get; set; }
}

public class VacancyTag : BaseEntity
{
    public Tag Tag { get; set; }
    public long TagId { get; set; }

    public Vacancy Vacancy { get; set; }
    public long VacancyId { get; set; }
}

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public ICollection<VacancyTag> VacancyTags { get; set; }
}

public class JobVacancy : Vacancy
{
    public int RequiredYearOfExperience { get; set; }
    public WorkSchedule WorkSchedule { get; set; }

}

public class EducationVacancy : Vacancy
{
    public DateTime ClassStartDate { get; set; }
    public DateTime ClassEndDate { get; set; }
}

public enum WorkSchedule
{
    FullTime = 1,
    Flexible
}