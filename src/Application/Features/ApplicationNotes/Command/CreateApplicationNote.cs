using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.AplicationNotes.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.ApplicationNotes.Command;
public class CreateApplicationNoteHandler : IRequestHandler<CreatAplicationNoteCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateApplicationNoteHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreatAplicationNoteCommand request, CancellationToken cancellationToken)
    {
        var note = _mapper.Map<ApplicationNote>(request);
        await _context.Notes.AddAsync(note);
        return note.Id;
    }
}
