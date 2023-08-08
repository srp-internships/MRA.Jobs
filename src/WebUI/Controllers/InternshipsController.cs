﻿using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.Tests.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InternshipsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWithPagination([FromQuery] PaggedListQuery<InternshipVacancyListResponce> query)
    {
        var internships = await Mediator.Send(query);
        return Ok(internships);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewInternship(CreateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateInternship([FromRoute] Guid id, [FromBody] UpdateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteInternship([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteInternshipVacancyCommand { Id = id };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetInternshipById(string slug)
    {
        var internship = await Mediator.Send(new GetInternshipVacancyBySlugQuery { Slug = slug });
        return Ok(internship);
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] Guid id, [FromBody] AddTagToInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        request.InternshipId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] Guid id, [FromBody] RemoveTagFromInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        request.InternshipId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpPost("{id}/test")]
    public async Task<ActionResult<TestInfoDTO>> SendTestCreationRequest([FromRoute] Guid id, [FromBody] CreateTestCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/test/result")]
    public async Task<ActionResult<TestResultDTO>> GetTestResultRequest([FromRoute] Guid id, [FromBody] CreateTestResultCommand request, CancellationToken cancellationToken)
    {
        if (id != request.TestId)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
