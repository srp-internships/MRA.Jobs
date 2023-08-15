﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;

public class GetApplicationByIdQueryHandler : IRequestHandler<GetBySlugApplicationQuery, ApplicationDetailsDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetApplicationByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApplicationDetailsDto> Handle(GetBySlugApplicationQuery request,
        CancellationToken cancellationToken)
    {
        // var application = await _dbContext.Applications.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        Domain.Entities.Application application = await _dbContext.Applications
            .Include(a => a.History)
            .FirstOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);
        _ = application ?? throw new NotFoundException(nameof(Domain.Entities.Application), request.Slug);

        return _mapper.Map<ApplicationDetailsDto>(application);
    }
}