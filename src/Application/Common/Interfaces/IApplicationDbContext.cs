using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Domain.Entities;

public interface IApplicationDbContext
{
    public DbSet<Applicant> Applicants { get; }
    public DbSet<ApplicantSocialMedia> ApplicantSocialMedias { get; }
    public DbSet<VacancyTimelineEvent> VacancyTimelineEvents { get; }
    public DbSet<User> DomainUsers { get; }
    public DbSet<Reviewer> Reviewers { get; }
    public DbSet<JobVacancy> JobVacancies { get; }
    public DbSet<EducationVacancy> EducationVacancies { get; }
    public DbSet<Vacancy> Vacancies { get; }
    public DbSet<VacancyCategory> Categories { get; }
    public DbSet<VacancyTag> VacancyTags { get; }
    public DbSet<UserTag> UserTags { get; }
    public DbSet<UserTimelineEvent> UserTimelineEvents { get; }
    public DbSet<TimelineEvent> TimelineEvents { get; }
    public DbSet<Test> Tests { get; }
    public DbSet<TestResult> TestResults { get; }
    public DbSet<Tag> Tags { get; }
    public DbSet<Domain.Entities.Application> Applications { get; }
    public DbSet<ApplicationTimelineEvent> ApplicationTimelineEvents { get; }
    public DbSet<Internship> Internships { get; }
    public DbSet<TrainingModel> TrainingModels { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
