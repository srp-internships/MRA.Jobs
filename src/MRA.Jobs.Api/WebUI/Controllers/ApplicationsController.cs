using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.Delete;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ApplicationsController : ApiControllerBase
{
    private readonly IFileService _fileService;

    public ApplicationsController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("DownloadCv/{key}")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadCv(string key)
    {
        if (!await _fileService.FileExistsAsync(key))
            return NotFound("File not found.");

        var fileBytes = await _fileService.Download(key);
        return File(fileBytes, "application/octet-stream", key);
    }


    [HttpGet]
    public async Task<ActionResult<PagedList<ApplicationListDto>>> GetAll(
        [FromQuery] PagedListQuery<ApplicationListDto> query)
    {
        var applications = await Mediator.Send(query);
        return Ok(applications);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ApplicationDetailsDto>> Get(string slug, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetBySlugApplicationQuery { Slug = slug }, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateApplication(CreateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<Guid>> UpdateApplication(string slug, UpdateApplicationCommand request,
        CancellationToken cancellationToken)
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
    public async Task<ActionResult<bool>> UpdateStatus(string slug, UpdateApplicationStatus request,
        CancellationToken cancellationToken)
    {
        request.Slug = slug;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/add-note")]
    public async Task<ActionResult<bool>> AddNote(string slug, AddNoteToApplicationCommand request,
        CancellationToken cancellationToken)
    {
        request.Slug = slug;
        return await Mediator.Send(request, cancellationToken);
    }
}