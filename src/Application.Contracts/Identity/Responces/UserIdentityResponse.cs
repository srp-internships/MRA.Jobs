namespace MRA.Jobs.Application.Contracts.Identity.Responces;

public class UserIdentityResponse
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public IEnumerable<string> Permissions { get; set; }
    public bool TwoFactorEnabled { get; set; }
}