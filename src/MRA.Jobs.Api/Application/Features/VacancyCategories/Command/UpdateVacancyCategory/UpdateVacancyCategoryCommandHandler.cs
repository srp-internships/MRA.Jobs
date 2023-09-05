using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;
public class UpdateVacancyCategoryCommandHandler : IRequestHandler<UpdateVacancyCategoryCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISlugGeneratorService _slugService;

    public UpdateVacancyCategoryCommandHandler(IApplicationDbContext context, IMapper mapper, ISlugGeneratorService slugService)
    {
        _context = context;
        _mapper = mapper;
        _slugService = slugService;
    }
    public async Task<string> Handle(UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(e => e.Slug == request.Slug, cancellationToken)
            ?? throw new NotFoundException(nameof(VacancyCategory), request.Slug);

        _mapper.Map(request, entity);
        var result = _context.Categories.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Slug;
    }
}
