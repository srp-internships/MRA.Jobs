using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MRA.Identity.Infrastructure.Persistence;
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
        if (_configuration["Environment"] == "Staging") _context.Database.EnsureDeleted();

        if (_context.Database.IsSqlServer())
        {
            await _context.Database.MigrateAsync();
        }
    }
}