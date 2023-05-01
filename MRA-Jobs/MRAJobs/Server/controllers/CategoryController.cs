using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Abstractions;
using MRA_Jobs.Domain.Entities;

namespace MRAJobs.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : GenericController<Category>
{
    public CategoryController(IEntityService<Category> service) : base(service)
    {

    }
}
