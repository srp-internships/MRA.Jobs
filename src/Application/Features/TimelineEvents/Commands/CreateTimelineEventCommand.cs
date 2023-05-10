using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.TimelineEvents.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.TimelineEvents.Commands;
public class CreateTimelineEventCommandHandler : IRequestHandler<CreateTimelineEventCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTimelineEventCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<long> Handle(CreateTimelineEventCommand request, CancellationToken cancellationToken)
    {
        var timelineEvent = _mapper.Map<TimelineEvent>(request);
        var result = await _context.TimelineEvents.AddAsync(timelineEvent);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
public class TimelineEventCommandValidator:AbstractValidator<CreateTimelineEventCommand>
{
    public TimelineEventCommandValidator()
    {
        //RuleFor(t=>t.Note).NotEmpty().WithMessage("this property cannot be empty");
        //RuleFor(t => t.CreateByUserId).NotNull().WithMessage("this property cannot be null");
    }
}
