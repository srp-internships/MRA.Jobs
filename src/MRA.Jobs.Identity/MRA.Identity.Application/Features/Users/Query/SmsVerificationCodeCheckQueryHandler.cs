using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class SmsVerificationCodeCheckQueryHandler : IRequestHandler<SmsVerificationCodeCheckQuery, SmsVerificationCodeStatus>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public SmsVerificationCodeCheckQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<SmsVerificationCodeStatus> Handle(SmsVerificationCodeCheckQuery request, CancellationToken cancellationToken)
    {
        request.PhoneNumber = request.PhoneNumber.Trim();
        if (request.PhoneNumber.Length == 9) request.PhoneNumber = "+992" + request.PhoneNumber.Trim();
        else if (request.PhoneNumber.Length == 12 && request.PhoneNumber[0] != '+') request.PhoneNumber = "+" + request.PhoneNumber;

        var expirationTime = DateTime.Now.AddMinutes(-1);

        var result = _context.ConfirmationCodes
            .Any(c => c.Code == request.Code && c.PhoneNumber == request.PhoneNumber && c.SentAt >= expirationTime);

        if (!result)
            return SmsVerificationCodeStatus.CodeVerifyFailure;

        return SmsVerificationCodeStatus.CodeVerifySuccess;
    }
}
