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

    public CreateCVCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(CreateCVCommand request, CancellationToken cancellationToken)
    {
        foreach (var ed in request.EducationDetails)
        {
            ed.UserId = request.UserId;
            await _context.EducationDetails.AddAsync(ed);
        }
        foreach (var ed in request.ExperienceDetails)
        {
            ed.UserId = request.UserId;
            await _context.ExperienceDetails.AddAsync(ed);
        }
        foreach (var skill in request.Skills)
        {
            skill.UserId = request.UserId;
            await _context.Skills.AddAsync(skill);
        }
        await _context.SaveChangesAsync(cancellationToken);

        return request.UserId;
    }
}
