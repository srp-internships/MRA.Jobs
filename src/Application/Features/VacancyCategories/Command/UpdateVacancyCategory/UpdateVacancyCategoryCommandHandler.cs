using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;
public class UpdateVacancyCategoryCommandHandler : IRequestHandler<UpdateVacancyCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateVacancyCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync(request.Id, cancellationToken);
        _ = entity ?? throw new NotFoundException(nameof(VacancyCategory), request.Id);
        var vacancyCategory = _mapper.Map<VacancyCategory>(request);
        var result = _context.Categories.Update(vacancyCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
