using MRA.Jobs.Application.Contracts.Internships.Commands;

namespace MRA.Jobs.Application.Features.Internships.Command.UpdateInternship;
public class UpdateInternshipCommandHandler : IRequestHandler<UpdateInternshipCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateInternshipCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateInternshipCommand request, CancellationToken cancellationToken)
    {
        var internship = await _context.Internships.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Internship), request.Id);
        internship = _mapper.Map<Internship>(request);
        var result = _context.Internships.Update(internship);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}