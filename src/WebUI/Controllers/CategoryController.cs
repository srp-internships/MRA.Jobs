namespace MRA.Jobs.Web.Controllers;

public class CategoryController : ApiControllerBase
{
    private readonly ILogger<CategoryController> _logger;

    [HttpGet("{id}")]
    public async Task<ActionResult<VacancyCategoryListDTO>> GetVacancyCategory(Guid id)
    public CategoryController(ILogger<CategoryController> logger)
    {
        var query = new GetVacancyCategoryByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateVacancyCategory(CreateVacancyCategoryCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> UpdateVacancyCategory(Guid id, UpdateVacancyCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteVacancyCategory(Guid id)
    {
        var command = new DeleteVacancyCategoryCommand { Id = id };
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
