﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;

namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<ApplicantSocialMedia> ApplicantSocialMedias { get; set; }
    public DbSet<VacancyTimelineEvent> VacancyTimelineEvents { get; set; }
    public DbSet<JobVacancy> JobVacancies { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<VacancyCategory> Categories { get; set; }
    public DbSet<VacancyTag> VacancyTags { get; set; }
    public DbSet<UserTag> UserTags { get; set; }
    public DbSet<UserTimelineEvent> UserTimelineEvents { get; set; }
    public DbSet<TimelineEvent> TimelineEvents { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestResult> TestResults { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Domain.Entities.Application> Applications { get; set; }
    public DbSet<ApplicationTimelineEvent> ApplicationTimelineEvents { get; set; }
    public DbSet<InternshipVacancy> Internships { get; set; }
    public DbSet<TrainingVacancy> TrainingVacancies { get; set; }
    public DbSet<EducationDetail> EducationDetails { get; set; }
    public DbSet<ExperienceDetail> ExperienceDetails { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<VacancyQuestion> VacancyQuestions { get; set; }
    public DbSet<VacancyResponse> VacancyResponses { get; set; }
    public DbSet<VacancyTaskDetail> VacancyTaskDetails { get; set; }
    public DbSet<TaskResponse> TaskResponses { get; set; }
    public DbSet<VacancyTask> VacancyTasks { get; set; }

    #region override

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // builder.Entity<BaseEntity>().HasQueryFilter(e => !e.IsDeleted);
        builder.Ignore<BaseEntity>();
        builder.Ignore<BaseAuditableEntity>();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await _mediator.DispatchDomainEvents(this); //Right now we do not need domain events.
        foreach (EntityEntry entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDelete))
        {
            entry.State = EntityState.Modified;
            ((ISoftDelete)entry.Entity).IsDeleted = true;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
