using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Queries;
using MRA.Identity.Application.Contract.UserRoles.Commands;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using Azure.Identity;

namespace MRA.Identity.Api.Controllers;
//[Authorize(Roles = "RoleManager")]
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserRolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string role = null, string userName =null)
    {
        var query = new GetUserRolesQuery { Role = role, UserName = userName };
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
            return BadRequest(result.Exception);

        return BadRequest();
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var query = new GetUserRolesBySlugQuery(slug);
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
            return BadRequest(result.Exception);
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
        await _mediator.Send(command);
        return Ok();
    }
}
