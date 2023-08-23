namespace MRA.Jobs.Application.Contracts.MyProfile.Responses;

public class MyIdentityResponse
{
    public bool IsActive { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public IEnumerable<string> Permissions { get; set; }
    public bool TwoFactorEnabled { get; set; }
}