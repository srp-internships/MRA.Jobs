namespace MRA.Jobs.Application.Contracts.Reviewer.Command;

public class CreateReviewerCommand : IRequest<Guid>
{
    public string Avatar { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}