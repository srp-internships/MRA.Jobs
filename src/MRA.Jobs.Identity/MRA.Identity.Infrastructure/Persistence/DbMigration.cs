using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MRA.Identity.Infrastructure.Persistence;
public class DbMigration : IMigration
{
    private readonly ApplicationDbContext context;

    public DbMigration(ApplicationDbContext context)
    {
        this.context = context;
    }

    public void Migrate()
    {
        if (context.Database.IsRelational())
        {
            context.Database.Migrate();
        }
    }
}

public interface IMigration
{
    void Migrate();
}