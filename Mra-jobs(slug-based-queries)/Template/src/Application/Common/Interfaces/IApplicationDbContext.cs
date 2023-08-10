using MRA.JobsTemp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MRA.JobsTemp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
