using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Application.Contract.Educations.Command.Delete;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Experience.Command.Delete;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;
using MRA.Identity.Application.Contract.Experiences.Query;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Domain.Entities;

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


    [HttpGet("GetUserSkills/{userName}")]
    public async Task<IActionResult> GetUserSkills([FromRoute] string userName = null)
    {
        var result = await _mediator.Send(new GetUserSkillsQuery { UserName = userName });
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpDelete("RemoveUserSkill/{skill}")]
    public async Task<IActionResult> RemoveUserSkill([FromRoute] string skill)
    {
        var result = await _mediator.Send(new RemoveUserSkillCommand { Skill = skill });
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpPost("CreateEducationDetail")]
    public async Task<IActionResult> CreateEducationDetail([FromBody] CreateEducationDetailCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpDelete("DeleteEducationDetail")]
    public async Task<IActionResult> DeleteEducationDetail([FromQuery] DeleteEducationCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpPut("UpdateEducationDetail")]
    public async Task<IActionResult> UpdateEducationDetail([FromBody] UpdateEducationDetailCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpGet("GetEducationsByUser")]
    public async Task<IActionResult> GetEducationsByUser([FromQuery] GetEducationsByUserQuery query)
    {
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpPost("СreateExperienceDetail")]
    public async Task<IActionResult> CreateExperienceDetail([FromBody] CreateExperienceDetailCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpPut("UpdateExperienceDetail")]
    public async Task<IActionResult> UpdateExperienceDetail([FromBody] UpdateExperienceDetailCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpDelete("DeleteExperienceDetail")]
    public async Task<IActionResult> DeleteExperienceDetail([FromQuery] DeleteExperienceCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }

    [HttpGet("GetExperiencesByUser")]
    public async Task<IActionResult> GetExperiencesByUser([FromQuery] GetExperiencesByUserQuery query)
    {
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
            return BadRequest(result.Exception + result.ErrorMessage);
        return Ok(result);
    }
}