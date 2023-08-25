using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TagsController : ApiControllerBase
{

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
}
