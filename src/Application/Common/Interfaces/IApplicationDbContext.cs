using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<JobVacancy> JobVacancies { get; }
    DbSet<VacancyCategory> Categories { get; }
    DbSet<Application> Applications { get; }
    DbSet<Applicant> Applicants { get; }
    DbSet<Vacancy> Vacancies { get; }
    DbSet<User> Users { get; }
    DbSet<Tag> Tags { get; }
    DbSet<VacancyTag> VacancyTags { get; }
    DbSet<UserTag> UserTags { get; }
    DbSet<TimelineEvent> TimelineEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
