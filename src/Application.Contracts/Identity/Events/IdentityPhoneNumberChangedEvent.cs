namespace MRA.Jobs.Application.Contracts.Identity.Events;

public class IdentityPhoneNumberChangedEvent : INotification
{
    public Guid Id { get; set; }
    public string NewPhoneNumber { get; set; }
}