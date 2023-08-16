using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrainingsController : ApiControllerBase
{

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetTrainingVacancyById(string slug)
    {
        var training = await Mediator.Send(new GetTrainingVacancyBySlugQuery { Slug = slug });
        return Ok(training);
    } 
    [HttpGet("/GetWithIf/{slug}")]
    public async Task<IActionResult> GetTrainingVacancyByIdSlugSinceCheckDate(string slug)
    {
        var training = await Mediator.Send(new GetTrairaingVacancyBySlugSinceCheckDate { Slug = slug });
        return Ok(training);
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<TrainingVacancyListDto>>> GetTrainingVacancysWithPagination([FromQuery] PagedListQuery<TrainingVacancyListDto> query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewTrainingVacancy(CreateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<Guid>> UpdateTrainingVacancy([FromRoute] string slug, [FromBody] UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> DeleteTrainingVacancy([FromRoute] string slug, [FromBody] DeleteTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] string slug, [FromBody] AddTagToTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        request.TrainingVacancySlug = slug;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{slug}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] string slug, [FromBody] RemoveTagFromTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        request.TrainingVacancySlug = slug;
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
