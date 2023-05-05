using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Internships.Queries;
using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships.Query;
public class GetInternshipById : IRequestHandler<GetInternshipByIdQuery, GetInternshipByIdResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInternshipById(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetInternshipByIdResponse> Handle(GetInternshipByIdQuery request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships
            .FirstOrDefaultAsync(i => i.Id == request.Id);

        return _mapper.Map<GetInternshipByIdResponse>(internship);
    }
}
