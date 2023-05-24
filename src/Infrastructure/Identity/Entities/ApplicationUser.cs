using Microsoft.AspNetCore.Identity;

namespace MRA.Jobs.Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser() : base()
    {
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}

