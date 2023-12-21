using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<ApplicantSocialMedia> ApplicantSocialMedias { get; }
    public DbSet<VacancyTimelineEvent> VacancyTimelineEvents { get; }
    public DbSet<JobVacancy> JobVacancies { get; }
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
    public DbSet<InternshipVacancy> Internships { get; }
    public DbSet<TrainingVacancy> TrainingVacancies { get; }
    public DbSet<EducationDetail> EducationDetails { get; set; }
    public DbSet<ExperienceDetail> ExperienceDetails { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<VacancyQuestion> VacancyQuestions { get; set; }
    public DbSet<VacancyResponse> VacancyResponses { get; set; }
    public DbSet<VacancyTask> VacancyTasks { get; set; }
    public DbSet<VacancyTaskDetail> VacancyTaskDetails { get; set; }
    public DbSet<TaskResponse> TaskResponses { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}