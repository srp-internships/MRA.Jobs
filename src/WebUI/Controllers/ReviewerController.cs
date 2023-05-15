using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Web.Controllers;

public class ReviewerController : ApiControllerBase
{
    private readonly ILogger<ApplicantController> _logger;

    public ReviewerController(ILogger<ApplicantController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewerDetailsDTO>> GetApplicantById(GetReviewerByIdQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApplicant([FromQuery] PaggedListQuery<ReviewerListDTO> request)
    {
        var applicants = await Mediator.Send(request);
        return Ok(applicants);
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