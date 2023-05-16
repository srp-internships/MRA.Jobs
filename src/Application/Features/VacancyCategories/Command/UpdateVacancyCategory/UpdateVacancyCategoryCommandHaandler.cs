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
        var entity = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
        _ = entity ?? throw new NotFoundException(nameof(VacancyCategory), request.Id);
        _mapper.Map(request, entity);

        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}