namespace MRA.Jobs.Infrastructure.Identity.Entities;

public class PhoneNumberVerificationCode
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsUsed { get; set; }
}