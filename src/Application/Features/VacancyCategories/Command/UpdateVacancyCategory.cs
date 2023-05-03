using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command;
public class UpdateVacancyCategoriesCommandHandler : IRequestHandler<UpdateVacancyCategoryCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateVacancyCategoriesCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<long> Handle(UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
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
