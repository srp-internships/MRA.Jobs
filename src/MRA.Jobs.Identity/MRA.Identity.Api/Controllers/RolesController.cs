using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Queries;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetRolesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var query = new GetRoleBySlugQuery { Slug = slug };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Update(UpdateRoleCommand command, string slug)
    {
        command.Slug = slug;
        var result= await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var command = new DeleteRoleCommand { Slug = slug };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}



