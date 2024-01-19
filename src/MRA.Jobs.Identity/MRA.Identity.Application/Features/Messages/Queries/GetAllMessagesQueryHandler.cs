﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.Messages.Queries;
using MRA.Identity.Application.Contract.Messages.Responses;

namespace MRA.Identity.Application.Features.Messages.Queries;
public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, List<GetMessageResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllMessagesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<GetMessageResponse>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = _context.Messages.OrderByDescending(m => m.Date).Select(m => _mapper.Map<GetMessageResponse>(m)).ToList(); 
        return messages;
    }
}
