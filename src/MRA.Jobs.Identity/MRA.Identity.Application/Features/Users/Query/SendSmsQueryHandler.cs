using MediatR;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Application.Features.Users.Query;
public class SendSmsQueryHandler : IRequestHandler<SendSmsQuery, string>
{
    private readonly ISmsService _smsService;

    public SendSmsQueryHandler(ISmsService smsService)
    {
        _smsService = smsService;
    }
    public async Task<string> Handle(SendSmsQuery request, CancellationToken cancellationToken)
    {
        var response = await _smsService.SendSms(request.PhoneNumber);
        return response;
    }
}