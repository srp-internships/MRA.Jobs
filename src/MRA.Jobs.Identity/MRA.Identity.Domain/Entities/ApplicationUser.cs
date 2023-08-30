using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public virtual Collection<RefreshToken> RefreshTokens { get; set; }
    public ApplicationUser()
    {
        RefreshTokens = new Collection<RefreshToken>();
    }
}