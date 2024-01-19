using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Configurations.OsonSms.SmsService;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Enums;
using MRA.Identity.Application.Contract.Messages.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Messages.Commands;
public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;
    private readonly ILogger<SmsService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserHttpContextAccessor _contextAccessor;

    public SendMessageCommandHandler(IApplicationDbContext context, ISmsService smsService, ILogger<SmsService> logger, IMapper mapper, IUserHttpContextAccessor contextAccessor)
    {
        _context = context;
        _smsService = smsService;
        _logger = logger;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }
    public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        request.Phone = request.Phone.Trim();
        if (request.Phone.Length == 9) request.Phone = "+992" + request.Phone.Trim();
        else if (request.Phone.Length == 12 && request.Phone[0] != '+') request.Phone = "+" + request.Phone;

        if (!System.Text.RegularExpressions.Regex.IsMatch(request.Phone, @"^\+992\d{9}$"))
        {
            throw new FormatException("Phone number does not follow the required format.");
        }

        var response = await _smsService.SendSmsAsync(request.Phone[1..], request.Text);

        try
        {
            if (response) request.Status = MessageStatusDto.Sent;
            else request.Status = MessageStatusDto.Failed;

            var message = _mapper.Map<Message>(request);

            message.SenderUserName = _contextAccessor.GetUserName();
            message.Id = Guid.NewGuid();
            message.Date = DateTime.Now;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }
}
