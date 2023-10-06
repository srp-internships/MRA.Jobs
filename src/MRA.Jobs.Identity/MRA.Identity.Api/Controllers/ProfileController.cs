using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Queries;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpPost("AddSkills")]
    public async Task<IActionResult> AddSkill([FromBody] AddSkillCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }


    [HttpGet("GetUserSkills")]
    public async Task<IActionResult> GetUserSkills([FromQuery] GetUserSkillsQuery query)
    {
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpDelete("RemoveUserSkill")]
    public async Task<IActionResult> RemoveUserSkill([FromBody] RemoveUserSkillCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }
}
