
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ApiControllerBase
{
    //private readonly ILogger<OidcConfigurationController> _logger;

    //public CategoryController(ILogger<OidcConfigurationController> logger)
    //{
    //    _logger = logger;
    //}
    //[HttpGet]
    //public async Task<IActionResult> Get()
    //{
    //    var categories =await Mediator.Send(new GetVacancyCategoryQuery());
    //    return Ok(categories);
    //}
    //[HttpGet("{id}")]
    //public IActionResult Get(int id)
    //{
    //    var category = Mediator.Send(new GetVacancyCategoryByIdQuery { Id = id });
    //    return Ok(category);
    //}
}
