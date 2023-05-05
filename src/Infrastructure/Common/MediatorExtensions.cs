using MRA.Jobs.Domain.Common;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Common;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        await Task.CompletedTask;
        //var entities = context.ChangeTracker
        //    .Entries<BaseEntity>()
        //    .Where(e => e.Entity.DomainEvents.Any())
        //    .Select(e => e.Entity);

        //var domainEvents = entities
        //    .SelectMany(e => e.DomainEvents)
        //    .ToList();

        //entities.ToList().ForEach(e => e.ClearDomainEvents());

        //foreach (var domainEvent in domainEvents)
        //    await mediator.Publish(domainEvent);
    }
}
