using System;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Sieve;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries;

    public class GetInternshipVacanciesQueryHandler : IRequestHandler<GetInternshipsQueryOptions,
        PagedList<TrainingVacancyListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IApplicationSieveProcessor _sieveProcessor;

    public GetTrainingVacanciesQueryHandler(IApplicationDbContext context, IMapper mapper,
        IApplicationSieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
    }
}


