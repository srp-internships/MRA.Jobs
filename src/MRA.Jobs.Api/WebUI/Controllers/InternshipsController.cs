using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetWithPagination([FromQuery] PagedListQuery<InternshipVacancyListResponse> query)
    {
        var internships = await Mediator.Send(query);
        return Ok(internships);
    }

    [HttpPost]  
    public async Task<ActionResult<string>> CreateNewInternship(CreateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        var result= await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetInternshipBySlug), new { slug = result }, result);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<string>> UpdateInternship([FromRoute] string slug, [FromBody] UpdateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        var result= await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetInternshipBySlug), new { slug = result }, result);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> DeleteInternship([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var request = new DeleteInternshipVacancyCommand { Slug = slug };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetInternshipBySlug(string slug)
    {
        var internship = await Mediator.Send(new GetInternshipVacancyBySlugQuery { Slug = slug });
        return Ok(internship);
    }

    [HttpPost("{slug}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] string slug, [FromBody] AddTagToInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        request.InternshipSlug = slug;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{slug}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] string slug, [FromBody] RemoveTagFromInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        request.InernshipSlug = slug;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpPost("{slug}/test")]
    public async Task<ActionResult<TestInfoDto>> SendTestCreationRequest([FromRoute] string slug, [FromBody] CreateTestCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/test/result")]
    public async Task<ActionResult<TestResultDto>> GetTestResultRequest([FromRoute] string slug, [FromBody] CreateTestResultCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
