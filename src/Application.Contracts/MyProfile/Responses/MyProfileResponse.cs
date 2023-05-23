namespace MRA.Jobs.Application.Contracts.MyProfile.Responses;

public class MyProfileResponse
{
    public string Avatar { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Patronymic { get; set; }
    public Gender Gender { get; set; }
    public MyIdentityResponse Identity { get; set; }
}
