namespace MRA.Jobs.Application.Contracts.Identity.Events;
public class NewIdentityRegisteredEvent : INotification
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
}
