using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;

namespace MRA.Jobs.Web.Controllers;

public class ReviewersController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllReviewer()
    {
        var reviewers = await Mediator.Send(new GetAllReviewerQuery());
        return Ok(reviewers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewerById(Guid id)
    {
        var reviewer = await Mediator.Send(new GetReviewerByIdQuery { Id = id });
        return Ok(reviewer);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewReviewer(CreateReviewerCommand request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateReviewer([FromRoute] Guid id,
        [FromBody] UpdateReviewerCommand request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteReviewer(Guid id, [FromBody] DeleteReviewerCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AddTag(Guid id, [FromBody] AddTagsToReviewerCommand request, CancellationToken cancellationToken)
    {
        request.ReviewerId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}/tags")]
    public async Task<IActionResult> RemoveTags(Guid id, [FromBody] RemoveTagsFromReviewerCommand request, CancellationToken cancellationToken)
    {
        request.ReviewerId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}