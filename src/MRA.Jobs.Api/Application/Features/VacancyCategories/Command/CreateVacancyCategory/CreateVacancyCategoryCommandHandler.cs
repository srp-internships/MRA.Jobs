using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;

public class CreateVacancyCategoryCommandHandler : IRequestHandler<CreateVacancyCategoryCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISlugGeneratorService _slugService;

    public CreateVacancyCategoryCommandHandler(ISlugGeneratorService slugService, IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _slugService = slugService;
    }

    public async Task<string> Handle(CreateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var vacancyCategory = _mapper.Map<VacancyCategory>(request);
        vacancyCategory.Slug = GenerateSlug(vacancyCategory);
        await _context.Categories.AddAsync(vacancyCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return vacancyCategory.Slug;
    }
    private string GenerateSlug(VacancyCategory category)
    {
        var slug = _slugService.GenerateSlug($"{category.Name}");
        var count = _context.Categories.Count(c => c.Slug == slug);
        if (count == 0)
            return slug;

        return $"{slug}-{DateTime.Now.ToString("yyyyMMddHHmmss")}";
    }

}
