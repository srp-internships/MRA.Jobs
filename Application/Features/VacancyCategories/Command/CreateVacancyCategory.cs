using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command;
public class CreateVacancyCategoriesCommandHandler : IRequestHandler<CreateVacancyCategoryCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateVacancyCategoriesCommandHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<long> Handle(CreateVacancyCategoryCommand request, CancellationToken cancellationToken)
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
