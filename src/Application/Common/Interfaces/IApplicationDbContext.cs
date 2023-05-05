using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<JobVacancy> JobVacancies { get; }
    DbSet<VacancyCategory> Categories { get; }
    DbSet<Applicant> Applicants { get; }
    DbSet<User> Users { get; }
    DbSet<Internship> Internships { get; }
    DbSet<TrainingModel> TrainingModels { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
