using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.Delete;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationsByStatus;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure.Identity;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class  ApplicationsController : ApiControllerBase
{
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(ILogger<ApplicationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize(ApplicationPolicies.Reviewer)]
    public async Task<ActionResult<PagedList<ApplicationListDto>>> GetAll([FromQuery] PagedListQuery<ApplicationListDto> query)
    {
        var applications = await Mediator.Send(query);
        return Ok(applications);
    }

    [HttpGet("{slug}")]
    [Authorize(ApplicationPolicies.Reviewer)]
    public async Task<ActionResult<ApplicationDetailsDto>> Get(string slug, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetBySlugApplicationQuery { Slug = slug }, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateApplication(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("withoutApplicantId")]
    public async Task<ActionResult<Guid>> CreateApplicationWithoutApplicantId(CreateApplicationWithoutApplicantIdCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<Guid>> UpdateApplication(string slug, UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        request.Slug = slug;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> DeleteApplication(string slug, CancellationToken cancellationToken)
    {
        var request = new DeleteApplicationCommand { Slug = slug };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{slug}/update-status")]
    public async Task<ActionResult<bool>> UpdateStatus(string slug, UpdateApplicationStatus request, CancellationToken cancellationToken)
    {
        request.Slug = slug;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/add-note")]
    public async Task<ActionResult<bool>> AddNote(string slug, AddNoteToApplicationCommand request, CancellationToken cancellationToken)
    {
        request.Slug = slug;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{status}")]
    [Authorize(ApplicationPolicies.Reviewer)]
    public async Task<IActionResult> GetApplicationsByStatus(ApplicationStatus status)
    {
        var query = new GetApplicationsByStatusQuery { Status = status };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

}
