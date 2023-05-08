using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.CreateInternship;
public class CreateInternshipCommandHandler : IRequestHandler<CreateInternshipCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateInternshipCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = _mapper.Map<Internship>(request);
        var result = _context.Internships.Add(internship);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}