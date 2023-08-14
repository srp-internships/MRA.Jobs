namespace MRA.Jobs.Application.Contracts.Identity.Events;

public class IdentityEmailChangedEvent : INotification
{
    public Guid Id { get; set; }
    public string NewEmail { get; set; }
}
