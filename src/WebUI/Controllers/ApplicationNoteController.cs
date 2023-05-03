using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.AplicationNotes.Commands;

namespace MRA.Jobs.Web.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ApplicationNoteController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public ApplicationNoteController(ILogger<OidcConfigurationController> logger)
    {
        _logger = logger;
    }
    [HttpPost]
    public async Task<ActionResult<long>> Add(CreatAplicationNoteCommand noteCommand, CancellationToken cancellationToken)
    {
        return await Mediator.Send(noteCommand, cancellationToken);
    }
}