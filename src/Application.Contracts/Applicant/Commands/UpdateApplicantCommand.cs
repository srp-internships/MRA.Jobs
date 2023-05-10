using MediatR;

namespace MRA.Jobs.Application.Contracts.Applicant.Commands;

public class UpdateApplicantCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}