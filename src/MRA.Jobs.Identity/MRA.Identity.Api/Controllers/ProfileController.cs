using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.CV;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Application.Contract.Educations.Command.Delete;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Experience.Command.Delete;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;
using MRA.Identity.Application.Contract.Experiences.Queries;
using MRA.Identity.Application.Contract.Experiences.Query;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Infrastructure.PDF;

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
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetProfileByUserName([FromQuery] string userName = null)
    {
        var result = await _mediator.Send(new GetPofileQuery { UserName = userName });
        return Ok(result);
    }

    [HttpPost("AddSkills")]
    public async Task<IActionResult> AddSkill([FromBody] AddSkillsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("GetUserSkills")]
    public async Task<IActionResult> GetUserSkills([FromQuery] string userName = null)
    {
        var result = await _mediator.Send(new GetUserSkillsQuery { UserName = userName });
        return Ok(result);
    }

    [HttpGet("GetAllSkills")]
    public async Task<IActionResult> GetAllSkills()
    {
        var result = await _mediator.Send(new GetAllSkillsQuery());
        return Ok(result);
    }

    [HttpDelete("RemoveUserSkill/{skill}")]
    public async Task<IActionResult> RemoveUserSkill([FromRoute] string skill)
    {
        var result = await _mediator.Send(new RemoveUserSkillCommand { Skill = skill });
        return Ok(result);
    }

    [HttpPost("CreateEducationDetail")]
    public async Task<IActionResult> CreateEducationDetail([FromBody] CreateEducationDetailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("DeleteEducationDetail/{id}")]
    public async Task<IActionResult> DeleteEducationDetail([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteEducationCommand { Id = id });
        return Ok(result);
    }

    [HttpPut("UpdateEducationDetail")]
    public async Task<IActionResult> UpdateEducationDetail([FromBody] UpdateEducationDetailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("GetEducationsByUser")]
    public async Task<IActionResult> GetEducationsByUser([FromQuery] GetEducationsByUserQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("GetAllEducations")]
    public async Task<IActionResult> GetAllEducations()
    {
        var result = await _mediator.Send(new GetAllEducationsQuery());
        return Ok(result);
    }

    [HttpPost("СreateExperienceDetail")]
    public async Task<IActionResult> CreateExperienceDetail([FromBody] CreateExperienceDetailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("UpdateExperienceDetail")]
    public async Task<IActionResult> UpdateExperienceDetail([FromBody] UpdateExperienceDetailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("DeleteExperienceDetail/{id}")]
    public async Task<IActionResult> DeleteExperienceDetail([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteExperienceCommand { Id = id });
        return Ok(result);
    }

    [HttpGet("GetExperiencesByUser")]
    public async Task<IActionResult> GetExperiencesByUser([FromQuery] GetExperiencesByUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("GetAllExperiences")]

    public async Task<IActionResult> GetAllExperiences()
    {
        var result = await _mediator.Send(new GetAllExperienceQuery());
        return Ok(result);
    }

    [HttpGet("GenerateCV")]
    public async Task<ActionResult> GenerateCV()
    {
        var result = await _mediator.Send(new CVGenerateQuery());
        return File(result, "application/pdf", "cv.pdf");
    }

}