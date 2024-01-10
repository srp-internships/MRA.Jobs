using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.Infrastructure.Persistence;
public class DbMigration
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public DbMigration(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task InitialiseAsync()
    {
        if (_configuration["Environment"] == "Staging")
            await _context.Database.EnsureDeletedAsync();

        if (_context.Database.IsSqlServer())
            await _context.Database.MigrateAsync();
    }
}