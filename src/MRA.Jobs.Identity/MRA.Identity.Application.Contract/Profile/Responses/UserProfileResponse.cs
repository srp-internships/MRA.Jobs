
namespace MRA.Identity.Application.Contract.Profile.Responses;
public class UserProfileResponse
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AboutMyself { get; set; }

}
