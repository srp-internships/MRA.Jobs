using MediatR;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Application.Features.Users.Query;
public class SmsVerificationCodeCheckQueryHandler : IRequestHandler<SmsVerificationCodeCheckQuery, bool>
{
    private readonly ISmsService _smsService;

    public SmsVerificationCodeCheckQueryHandler(ISmsService smsService)
    {
        _smsService = smsService;
    }
    public async Task<bool> Handle(SmsVerificationCodeCheckQuery request, CancellationToken cancellationToken)
    {
        var response = await _smsService.CheckCode(request.PhoneNumber, request.Code);
        return response;
    }
}
