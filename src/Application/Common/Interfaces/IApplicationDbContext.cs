using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<JobVacancy> JobVacancies { get; }
    DbSet<VacancyCategory> Categories { get; }
    DbSet<Tag> Tags { get; }
    DbSet<User> DomainUsers { get; }
    DbSet<VacancyTag> VacancyTags { get; }
    DbSet<UserTag> UserTags { get; }
    DbSet<VacancyTimelineEvent> VacancyTimelineEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
