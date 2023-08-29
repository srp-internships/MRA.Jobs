using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.CV.Commands;

namespace MRA.Jobs.Application.Features.CV.Command.CreateCV;
public class CreateCVCommandHandler : IRequestHandler<CreateCVCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCVCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }
    public async Task<Guid> Handle(CreateCVCommand request, CancellationToken cancellationToken)
    {
        foreach (var ed in request.EducationDetails)
        {
            ed.UserId = request.UserId;
            await _context.EducationDetails.AddAsync(_mapper.Map<EducationDetail>(ed));
        }
        foreach (var ed in request.ExperienceDetails)
        {
            ed.UserId = request.UserId;
            await _context.ExperienceDetails.AddAsync(_mapper.Map<ExperienceDetail>(ed));
        }
        foreach (var skill in request.Skills)
        {
            skill.UserId = request.UserId;
            await _context.Skills.AddAsync(_mapper.Map<Skill>(skill));
        }
        await _context.SaveChangesAsync(cancellationToken);

        return request.UserId;
    }
}
