﻿using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationsController : ApiControllerBase
{
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(ILogger<ApplicationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ApplicationListDto>>> GetAll(
        [FromQuery] PagedListQuery<ApplicationListDto> query)
    {
        PagedList<ApplicationListDto> applications = await Mediator.Send(query);
        return Ok(applications);
    }

    [HttpGet("{Id:guid}")]
    public async Task<ActionResult<ApplicationDetailsDto>> GetApplicationById(GetByIdApplicationQuery request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }


    [HttpPost]
    public async Task<ActionResult<Guid>> CreateApplication(CreateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("withoutApplicantId")]
    public async Task<ActionResult<Guid>> CreateApplicationWithoutApplicantId(
        CreateApplicationWithoutApplicantIdCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<Guid>> UpdateApplication(Guid Id, UpdateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult<bool>> DeleteApplication(Guid Id, CancellationToken cancellationToken)
    {
        DeleteApplicationCommand request = new DeleteApplicationCommand { Id = Id };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{Id}/update-status")]
    public async Task<ActionResult<bool>> UpdateStatus(Guid Id, UpdateApplicationStatus request,
        CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{Id}/add-note")]
    public async Task<ActionResult<bool>> AddNote(Guid Id, AddNoteToApplicationCommand request,
        CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{status}")]
    public async Task<IActionResult> GetApplicationsByStatus(ApplicationStatus status)
    {
        GetApplicationsByStatusQuery query = new GetApplicationsByStatusQuery { Status = status };
        List<ApplicationListStatus> result = await Mediator.Send(query);
        return Ok(result);
    }
}