using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Vacancy.Queries;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class VacanciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VacanciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vacancy>>> GetFilteredVacancies([FromQuery] int? requiredYearOfExperience, [FromQuery] WorkSchedule? workSchedule, [FromQuery] Guid? categoryId, [FromQuery] string title)
    {
        var vacancies = await _mediator.Send(new GetFilteredVacanciesQuery(requiredYearOfExperience, workSchedule, categoryId, title));
        return Ok(vacancies);
    }
}
