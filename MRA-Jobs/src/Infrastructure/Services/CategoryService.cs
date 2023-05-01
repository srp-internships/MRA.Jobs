using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MRA_Jobs.Application.Common.Interfaces;
using MRA_Jobs.Application.Common.Models;
using MRA_Jobs.Domain.Entities;
using MRA_Jobs.Infrastructure.Persistence;

namespace MRA_Jobs.Infrastructure.Services;
public class CategoryService : EntityService<Category>, ICategoryService
{
    private ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context,IMapper mapper)
        :base(context,mapper)
    {
        _context = context;
    }
   
}
