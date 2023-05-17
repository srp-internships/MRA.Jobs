using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Web.Controllers;

public class ReviewerController : ApiControllerBase
{
    public ReviewerController()
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviewer()
    {
        var reviewers = await Mediator.Send(new GetAllReviewerQuery());
        return Ok(reviewers);
    }

    [HttpGet("{id}")]
    public  IActionResult GetReviewerById(Guid id)
    {
        var reviewer = Mediator.Send(new GetReviewerByIdQuery { Id = id });
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
    public async Task<ActionResult<bool>> DeleteApplicant(Guid id, [FromBody] DeleteReviewerCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }
    
}