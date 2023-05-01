using AutoMapper;
using MRA_Jobs.Application.Common.Interfaces;
using MRA_Jobs.Domain.Entities;
using MRA_Jobs.Infrastructure.Persistence;

namespace MRA_Jobs.Infrastructure.Services;
public class CategoryService : EntityService<Category>, ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context, IMapper mapper)
        : base(context, mapper)
    {
        _context = context;
    }
   
}
