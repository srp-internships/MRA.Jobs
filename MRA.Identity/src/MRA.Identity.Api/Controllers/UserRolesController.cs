using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public UserRolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "RoleManager")]
    public IActionResult GetUserRoles([FromQuery] string username)
    {
        var userRoles = _userManager.Users.Select(u => new
        {
            UserName = u.UserName,
            Roles = _userManager.GetRolesAsync(u).Result

        });

        return Ok(userRoles);
    }

    [HttpGet("{slug}")]
    [Authorize(Roles = "RoleManager")]
    public IActionResult GetSingleRole(string slug)
    {
        var role = _roleManager.Roles.FirstOrDefault(r => r.Name.ToLower() == slug.ToLower());
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost]
    [Authorize(Roles = "RoleManager")]
    public async Task<IActionResult> AddUserRole([FromBody] UserRoleDto userRoleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var role = await _roleManager.FindByIdAsync(userRoleDto.RoleId);
        if (role == null)
        {
            return NotFound("Role not found");
        }

        var result = await _userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpDelete("{slug}")]
    [Authorize(Roles = "RoleManager")]
    public async Task<IActionResult> RemoveUserRole(string slug)
    {
        var user = await _userManager.FindByIdAsync(slug);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            return BadRequest("User does not have any roles");
        }

        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}

public class UserRoleDto
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
}

