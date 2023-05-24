using Microsoft.AspNetCore.Identity;

namespace MRA.Jobs.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static AuthResult ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? AuthResult.Success()
            : AuthResult.Failure(result.Errors.Select(e => e.Description));
    }
}
