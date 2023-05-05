using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command;
public class CreateVacancyCategoriesCommandHandler : IRequestHandler<CreateVacancyCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateVacancyCategoriesCommandHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(CreateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var vacancyCategory = _mapper.Map<VacancyCategory>(request);
        var result = _context.Categories.Add(vacancyCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}

public class CreateVacancyCategoriesCommandValidator : AbstractValidator<CreateVacancyCategoryCommand>
{
    public CreateVacancyCategoriesCommandValidator()
    {
        RuleFor(s => s.Name).NotEmpty();
       
    }
}
