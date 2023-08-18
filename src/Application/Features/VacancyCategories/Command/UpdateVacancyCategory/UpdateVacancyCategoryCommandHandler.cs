﻿using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;
public class UpdateVacancyCategoryCommandHandler : IRequestHandler<UpdateVacancyCategoryCommand, Guid>
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
    public async Task<Guid> Handle(UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(e => e.Slug == request.Slug, cancellationToken)
            ?? throw new NotFoundException(nameof(VacancyCategory), request.Slug);

        _mapper.Map(request, entity);
        entity.Slug = _slugService.GenerateSlug(request.Name);
        var result = _context.Categories.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
