using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Application.Contracts.Vacancies.Queries;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;
using MRA.Jobs.Infrastructure.Identity;

namespace MRA.Jobs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(ApplicationPolicies.Reviewer)]
    public class VacanciesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllVacanciesQuery()));
        }
        
        
        [HttpPut("ChangeNote")]
        public async Task<ActionResult<bool>> ChangeNote([FromBody] ChangeVacancyNoteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("{id}/tags")]
        public async Task<ActionResult<List<string>>> AddTags([FromRoute] Guid id,
            [FromBody] AddTagsToVacancyCommand command)
        {
            command.VacancyId = id;
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}/tags/")]
        public async Task<ActionResult<List<string>>> DeleteTags([FromRoute] Guid id,
            [FromBody] RemoveTagsFromVacancyCommand command)
        {
            command.VacancyId = id;
            return Ok(await Mediator.Send(command));
        }
    }
}