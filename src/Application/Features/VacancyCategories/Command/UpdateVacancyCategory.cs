using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command;
public class UpdateVacancyCategoriesCommandHandler : IRequestHandler<UpdateVacancyCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateVacancyCategoriesCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.JobVacancies.FindAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(JobVacancy), request.Id);
        var vacancyCategory = _mapper.Map<VacancyCategory>(request);
        var result = _context.Categories.Update(vacancyCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}

public class UpdateVacancyCategoriesCommandValidator : AbstractValidator<UpdateVacancyCategoryCommand>
{
    public UpdateVacancyCategoriesCommandValidator()
    {
        RuleFor(s => s.Name).NotEmpty();

    }
}
