using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;
public class CreateVacancyCategoryCommandHandler : IRequestHandler<CreateVacancyCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateVacancyCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }
    public async Task<Guid> Handle(CreateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var vacancyCategory = _mapper.Map<VacancyCategory>(request);
        await _context.Categories.AddAsync(vacancyCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vacancyCategory.Id;
    }
}
