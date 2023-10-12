using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MRA.Identity.Application.Contract.UserRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Commands;

namespace MRA.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(ApplicationPolicies.Administrator)]
public class UserRolesController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserRolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string role = null, string userName = null)
    {
        return Ok(await _mediator
        .Send(new GetUserRolesQuery
        {
            Role = role,
            UserName = userName
        }));  
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var query = new GetUserRolesBySlugQuery { Slug = slug };
        var result = await _mediator.Send(query);
        if (result.IsSuccess)
        {
            return Ok(result.Response);
        }
        if (result.ErrorMessage != null)
        {
            return BadRequest(result.ErrorMessage);
        }
        if (result.Exception != null)
            return BadRequest(result.Exception.ToString());
        return BadRequest();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRolesCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            return Ok(result.Response);
        }
        if (result.ErrorMessage != null)
        {
            return BadRequest(result.ErrorMessage);
        }
        if (result.Exception != null)
            return BadRequest(result.Exception.ToString());
        return BadRequest();
    }
    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var command = new DeleteUserRoleCommand { Slug = slug };
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);
        return Ok(result);
    }
}
