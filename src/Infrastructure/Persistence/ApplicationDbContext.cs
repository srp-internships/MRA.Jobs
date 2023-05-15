using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MRA.Jobs.Infrastructure.Persistence.Interceptors;

namespace MRA.Jobs.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    internal ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions) :
        base(options, operationalStoreOptions)
    {

    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<ApplicantSocialMedia> ApplicantSocialMedias { get; set; }
    public DbSet<VacancyTimelineEvent> VacancyTimelineEvents { get; set; }
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }
    public DbSet<JobVacancy> JobVacancies { get; set; }
    public DbSet<EducationVacancy> EducationVacancies { get; set; }
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
    public DbSet<Internship> Internships { get; set; }
    public DbSet<TrainingModel> TrainingModels { get; set; }

    #region override
    protected override void OnModelCreating(ModelBuilder builder)
    {
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
        return await base.SaveChangesAsync(cancellationToken);
    }
    #endregion
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("dbsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"dbsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

        var connectionString = "data source=localhost; initial catalog=MRA_Jobs; integrated security=true; persist security info=true;TrustServerCertificate=True";
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        var operationalStoreOptions = new OperationalStoreOptions();
        var operationalStoreOptionsAccessor = Options.Create(operationalStoreOptions);

        return new ApplicationDbContext(optionsBuilder.Options, operationalStoreOptionsAccessor);
    }
}
