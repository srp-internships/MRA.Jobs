using MediatR;

namespace MRA.Jobs.Application.Contracts.User.Command;

public class CreateUserCommand : IRequest<long>
{
    public string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime UpdatedAt { get; set; }
}