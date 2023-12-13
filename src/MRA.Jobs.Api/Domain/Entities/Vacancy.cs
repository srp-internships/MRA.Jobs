﻿using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Domain.Entities;

[Index(nameof(Slug))]
public abstract class Vacancy : BaseAuditableEntity
{
    public string CreatedByEmail { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Slug { get; set; }

    public Guid CategoryId { get; set; }
    public VacancyCategory Category { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<VacancyTimelineEvent> History { get; set; }
    public ICollection<Test> Tests { get; set; }
    public IEnumerable<VacancyQuestion> VacancyQuestions { get; set; }
    public IEnumerable<VacancyTask> VacancyTasks { get; set; }
    public ICollection<VacancyTag> Tags { get; set; }
    public string Discriminator { get; set; }
}

