using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

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
    public async Task<ActionResult<PaggedList<ApplicationListDTO>>> GetAll([FromQuery] PaggedListQuery<ApplicationListDTO> query)
    {
        var applications = await Mediator.Send(query);
        return Ok(applications);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<ApplicationDetailsDTO>> GetApplicationById(GetByIdApplicationQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
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

    [HttpPut("{Id}")]
    public async Task<ActionResult<Guid>> UpdateApplication(Guid Id, UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult<bool>> DeleteApplication(Guid Id, CancellationToken cancellationToken)
    {
        var request = new DeleteApplicationCommand { Id = Id };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{Id}/update-status")]
    public async Task<ActionResult<bool>> UpdateStatus(Guid Id, UpdateApplicationStatus request, CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{Id}/add-note")]
    public async Task<ActionResult<bool>> AddNote(Guid Id, AddNoteToApplicationCommand request, CancellationToken cancellationToken)
    {
        request.Id = Id;
        return await Mediator.Send(request, cancellationToken);
    }
}
