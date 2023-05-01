using System.Reflection;

namespace MRA_Jobs.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<JobVacancy> JobVacancies { get; private set; }
    public DbSet<EducationVacancy> EducationVacancies { get; private set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=SQL8005.site4now.net; Database=mrajobs; User Id=db_a987fd_mrajobs_admin; Password=Lodkaspring2023; Encrypt=False;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}