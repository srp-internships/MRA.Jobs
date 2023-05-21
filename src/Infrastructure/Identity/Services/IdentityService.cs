using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Identity.Entities;

namespace MRA.Jobs.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<(AuthResult Result, Guid UserId)> CreateUserAsync(Guid userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName.ToString(),
            Email = userName.ToString(),
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string role)
    {
        //Remove after implementing auth
        return await Task.FromResult(true);

        //var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        //return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        //Remove after implementing auth
        return await Task.FromResult(true);

        //var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        //if (user == null)
        //{
        //    return false;
        //}

        //var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        //var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        //return result.Succeeded;
    }

    public async Task<AuthResult> DeleteUserAsync(Guid userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : AuthResult.Success();
    }

    public async Task<AuthResult> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}
