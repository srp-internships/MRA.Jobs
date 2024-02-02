using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Vacancies.Note.Commands;
using MRA.Jobs.Infrastructure.Identity;

namespace MRA.Jobs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(ApplicationPolicies.Reviewer)]
    public class VacanciesController : ApiControllerBase
    {
        [HttpPut("ChangeNote")]
        public async Task<ActionResult<bool>> ChangeNote([FromBody] ChangeVacancyNoteCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}