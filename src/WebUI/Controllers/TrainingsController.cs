using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrainingsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingVacancyById(Guid id)
    {
        return Ok(await Mediator.Send(new TrainingVacancyDetailedResponce { Id = id }));
    }

    [HttpGet]
    public async Task<ActionResult<PaggedList<TrainingVacancyListDTO>>> GetTrainingVacancysWithPagination([FromQuery] PaggedListQuery<TrainingVacancyListDTO> query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewTrainingVacancy(CreateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateTrainingVacancy([FromRoute] Guid id, [FromBody] UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTrainingVacancy([FromRoute] Guid id, [FromBody] DeleteTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] Guid id, [FromBody] AddTagToTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        request.VacancyId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] Guid id, [FromBody] RemoveTagFromTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        request.VacancyId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}
