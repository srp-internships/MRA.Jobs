﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MRA.Jobs.Domain.Entities;

public abstract class Vacancy : BaseAuditableEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public long CategoryId { get; set; }
    public VacancyCategory Category { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<VacancyTimelineEvent> VacancyTimelineEvents { get; set; }

    public ICollection<VacancyTag> Tags { get; set; }
}

public class JobVacancy : Vacancy
{
    public int RequiredYearOfExperience { get; set; }
    public WorkSchedule WorkSchedule { get; set; }

}
