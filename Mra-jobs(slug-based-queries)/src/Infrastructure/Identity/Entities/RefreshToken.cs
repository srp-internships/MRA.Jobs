namespace MRA.Jobs.Infrastructure.Identity.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    public string Token { get; set; }
    public DateTime ExpiryOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedByIp { get; set; }
    public string RevokedByIp { get; set; }
    public DateTime RevokedOn { get; set; }
}