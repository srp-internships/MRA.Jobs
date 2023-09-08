using MediatR;
using Microsoft.EntityFrameworkCore;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Application.Features.Users.Query;
public class SmsVerificationCodeCheckQueryHandler : IRequestHandler<SmsVerificationCodeCheckQuery, bool>
{
    private readonly ISmsService _smsService;
    private readonly IApplicationDbContext _context;

    public SmsVerificationCodeCheckQueryHandler(ISmsService smsService, IApplicationDbContext context)
    {
        _smsService = smsService;
        _context = context;
    }
    public async Task<bool> Handle(SmsVerificationCodeCheckQuery request, CancellationToken cancellationToken)
    {
        var expirationTime = DateTime.Now.AddMinutes(-1);

        var result = await _context.ConfirmationCodes
            .FirstOrDefaultAsync(c => c.Code == request.Code && c.PhoneNumber == request.PhoneNumber && c.SentAt >= expirationTime);

        if (result != null)
        {
            return true;
        }
        return false;

    }
}
