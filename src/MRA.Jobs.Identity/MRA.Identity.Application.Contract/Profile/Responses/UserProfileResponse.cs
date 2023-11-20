
namespace MRA.Identity.Application.Contract.Profile.Responses;
public class UserProfileResponse
{
    public string UserName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AboutMyself { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; init; }
    public bool PhoneNumberConfirmed { get; init; }

}
