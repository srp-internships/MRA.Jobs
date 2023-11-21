namespace MRA.Jobs.Infrastructure.Persistence;
public class DbMigration
{
    private readonly ApplicationDbContext _context;

    public DbMigration(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        if (_context.Database.IsSqlServer())
        {
            await _context.Database.MigrateAsync();
        }
    }
}