using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MRA.Jobs.Application.Contracts.Identity.Events;

namespace MRA.Jobs.Application.Features.Users.Events;

public class IdentityEmailChangedEventHandler : INotificationHandler<IdentityEmailChangedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<IdentityEmailChangedEventHandler> _logger;

    public IdentityEmailChangedEventHandler(IApplicationDbContext context,
        ILogger<IdentityEmailChangedEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task Handle(IdentityEmailChangedEvent notification, CancellationToken cancellationToken)
    {
        User entity = await _context.DomainUsers.FirstOrDefaultAsync(x => x.Id == notification.Id, cancellationToken);
        if (entity != null)
        {
            entity.Email = notification.NewEmail;
            _context.DomainUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            _logger.LogWarning("Applicant not found by identity id {Id}", notification.Id);
        }
    }
}