using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;

namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly IMediator _mediator;

    internal ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    // public DbSet<PhoneNumberVerificationCode> PhoneNumberVerificationCodes { get; set; }
    // public DbSet<RefreshToken> RefreshTokens { get; set; }
    // public DbSet<Permission> Permissions { get; set; }
    // public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<ApplicantSocialMedia> ApplicantSocialMedias { get; set; }
    public DbSet<VacancyTimelineEvent> VacancyTimelineEvents { get; set; }
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }
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

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        string basePath = Directory.GetCurrentDirectory();
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("dbsettings.json", true, true)
            .AddJsonFile("dbsettings.Development.json", true, true)
            .Build();

        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}