using MRA.Jobs.Application.Contracts.Identity.Events;

namespace MRA.Jobs.Application.Features.Applicants.Events;

public class NewIdentityRegisteredEventHandler : INotificationHandler<NewIdentityRegisteredEvent>
{
    private readonly IApplicationDbContext _context;

    public NewIdentityRegisteredEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task Handle(NewIdentityRegisteredEvent notification, CancellationToken cancellationToken)
    {
        Applicant entity = new Applicant
        {
            Id = notification.Id,
            LastName = notification.LastName,
            FirstName = notification.FirstName,
            Patronymic = notification.Patronymic,
            Email = notification.Email,
            DateOfBirth = notification.DateOfBirth,
            PhoneNumber = notification.PhoneNumber,
            Gender = notification.Gender
        };
        await _context.Applicants.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}