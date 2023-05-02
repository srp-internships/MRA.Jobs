using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA_Jobs.Application.Common.Models;
using MRA_Jobs.Infrastructure.Persistence;

namespace MRA_Jobs.Infrastructure.Services;
public class ApplicationService : EntityService<Domain.Entities.Application>, IApplicationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ApplicationService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }
}
