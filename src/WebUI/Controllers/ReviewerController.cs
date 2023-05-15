using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Reviewer.Command;

namespace MRA.Jobs.Web.Controllers;

public class ReviewerController : ApiControllerBase
{
    public ReviewerController()
    {
        
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
}