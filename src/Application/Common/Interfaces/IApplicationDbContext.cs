using MRA.Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<JobVacancy> JobVacancies { get; }
    DbSet<VacancyCategory> Categories { get; }
    DbSet<User> Users { get; }
    DbSet<TimelineEvent> TimelineEvents { get; set; }
    DbSet<Tag> Tags { get; }
    DbSet<VacancyTag> VacancyTags { get; }
    DbSet<UserTag> UserTags { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
