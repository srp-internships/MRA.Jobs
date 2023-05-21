namespace MRA.Jobs.Application.Contracts.Reviewer.Command;

public class UpdateReviewerCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Avatar { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}