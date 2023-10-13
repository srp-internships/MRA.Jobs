using MediatR;

namespace MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
public class UpdateProfileCommand : IRequest<ApplicationResponse<bool>>
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string AboutMyself { get; set; }
}
