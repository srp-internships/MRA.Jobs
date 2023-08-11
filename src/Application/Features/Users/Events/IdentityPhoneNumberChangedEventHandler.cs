using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MRA.Jobs.Application.Contracts.Identity.Events;

namespace MRA.Jobs.Application.Features.Users.Events;

public class IdentityPhoneNumberChangedEventHandler : INotificationHandler<IdentityPhoneNumberChangedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<IdentityEmailChangedEventHandler> _logger;

    public IdentityPhoneNumberChangedEventHandler(IApplicationDbContext context,
        ILogger<IdentityEmailChangedEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task Handle(IdentityPhoneNumberChangedEvent notification, CancellationToken cancellationToken)
    {
        User entity = await _context.DomainUsers.FirstOrDefaultAsync(x => x.Id == notification.Id, cancellationToken);
        if (entity != null)
        {
            entity.PhoneNumber = notification.NewPhoneNumber;
            _context.DomainUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            _logger.LogWarning("Applicant not found by identity id {Id}", notification.Id);
        }
    }
}