﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;

namespace MRA.Jobs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ApiControllerBase
{
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<CategoryResponse>>> GetAll(
        [FromQuery] PagedListQuery<CategoryResponse> query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("training")]
    public async Task<IActionResult> GetTrainingCategories([FromQuery] GetTrainingCategoriesQuery query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("internship")]
    public async Task<IActionResult> GetInternshipCategories([FromQuery] GetInternshipCategoriesQuery query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var category = await Mediator.Send(new GetVacancyCategoryBySlugQuery { Slug = slug });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateNewCategoryVacancy(CreateVacancyCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }


    [HttpPut("{slug}")]
    public async Task<ActionResult<string>> Update([FromRoute] string slug,
        [FromBody] UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var command = new DeleteVacancyCategoryCommand { Slug = slug };
        await Mediator.Send(command);
        return NoContent();
    }
}