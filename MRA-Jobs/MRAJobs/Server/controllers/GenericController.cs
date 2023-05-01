using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Abstractions;

namespace MRAJobs.Server.Controllers;

public class GenericController<TEntity>:ControllerBase where TEntity : class
{
    private readonly IEntityService<TEntity> _service;

    public GenericController(IEntityService<TEntity> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
       var categories = await _service.GetAll<TEntity>();

       return Ok(categories);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _service.GetById<TEntity>(id);
        return Ok(category);
    }
    [HttpPost]
    public async Task<IActionResult> Add( TEntity entity)
    {
        var result = await _service.Add<TEntity, TEntity>(entity);
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult>Update( TEntity entity)
    {
        var result = await _service.Update<TEntity, TEntity>(entity);
        return Ok(result);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
      await  _service.Delete(id);
        return Ok();
    }
}
