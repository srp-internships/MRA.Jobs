using System.Threading;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Vacncies.Queries;
using MRA.Jobs.Application.Contracts.Vacncies.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class VacanciesController : ApiControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetWithPagination([FromQuery] PaggedListQuery<VacancyListDTO> query, CancellationToken cancellationToken)
    {
        var vacancies = await Mediator.Send(query, cancellationToken);
        return Ok(vacancies);
    }

    [HttpGet("byCategory")]
    public async Task<IActionResult> GetWithPaginationByCategory([FromQuery] PaggedListVacancyByCategory<VacancyListDTO> query, CancellationToken cancellationToken)
    {
        
        var vacancies = await Mediator.Send(query, cancellationToken);
        return Ok(vacancies);
    }

    [HttpGet("categoryVacancyCounts")]
    public async Task<IActionResult> GetCategoryVacancyCounts(CancellationToken cancellationToken)
    {
        var query = new GetCategoryVacancyCountsQuery();
        var categoryVacancyCounts = await Mediator.Send(query,cancellationToken);
        return Ok(categoryVacancyCounts);
    }
}
