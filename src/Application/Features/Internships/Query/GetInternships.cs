using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Internships.Queries;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships.Query;
public class GetInternships : IRequestHandler<GetInternshipsQuery, List<GetInternshipsResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternships(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<GetInternshipsResponse>> Handle(GetInternshipsQuery request, CancellationToken cancellationToken)
    {
        var internships = await _context.Internships
            .ToListAsync();

        return _mapper.Map<List<GetInternshipsResponse>>(internships);
    }
}
