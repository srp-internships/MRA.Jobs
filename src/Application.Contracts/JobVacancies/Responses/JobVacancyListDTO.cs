using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;

public class JobVacancyListDTO
{
    public long Id { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
}

public class JobVacancyDetailsDTO
{
    public long Id { get; set; }
    public long? CategoryId { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
    public int RequiredYearOfExperience { get; set; }

    public DateTime CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public long? LastModifiedBy { get; set; }
}

