using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.Tag.Commands;

namespace MRA.Jobs.Application.Features.Tag.Commands;

using MRA.Jobs.Domain.Entities;

partial class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {

        var entity = new  Tag()
        {
            Name = request.Name
        };
        _context.Tags.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
