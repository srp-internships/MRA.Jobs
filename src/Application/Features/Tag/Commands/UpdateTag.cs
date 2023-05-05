using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Tag.Commands;

namespace MRA.Jobs.Application.Features.Tag.Commands;
public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, long>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindAsync(request.Id, cancellationToken)
              ?? throw new NotFoundException(nameof(Tag), request.Id);

        entity.Name = request.Name;

        _context.Tags.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
