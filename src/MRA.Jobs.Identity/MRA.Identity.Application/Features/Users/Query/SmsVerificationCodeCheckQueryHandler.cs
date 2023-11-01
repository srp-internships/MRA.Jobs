using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mra.Shared.Common.Interfaces.Services;
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
        if (request.PhoneNumber[0] != '+') request.PhoneNumber = "+" + request.PhoneNumber.Trim();
        var expirationTime = DateTime.Now.AddMinutes(-1);

        var result = _context.ConfirmationCodes
            .Any(c => c.Code == request.Code && c.PhoneNumber == request.PhoneNumber && c.SentAt >= expirationTime);

        if (!result)
            return SmsVerificationCodeStatus.CodeVerifySuccess;

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

        if (user is null) return SmsVerificationCodeStatus.CodeVerifySuccess_ButUserDontSignUp;

        if (result)
        {
            user.PhoneNumberConfirmed = true;
            var saveResult = await _userManager.UpdateAsync(user);
            if (saveResult.Succeeded) return SmsVerificationCodeStatus.CodeVerifySuccess;
        }

        return SmsVerificationCodeStatus.CodeVerifyFailure;

    }
}
