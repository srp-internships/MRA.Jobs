using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;

public class
    SmsVerificationCodeCheckQueryHandler(IApplicationDbContext context, ISmsCodeChecker codeChecker)
    : IRequestHandler<SmsVerificationCodeCheckQuery, SmsVerificationCodeStatus>
{
    public async Task<SmsVerificationCodeStatus> Handle(SmsVerificationCodeCheckQuery request,
        CancellationToken cancellationToken)
    {
        bool result = codeChecker.VerifyPhone(request.Code, request.PhoneNumber);

        if (!result)
            return SmsVerificationCodeStatus.CodeVerifyFailure;


        var phoneNumber = request.PhoneNumber.Trim();
        phoneNumber = phoneNumber.Length switch
        {
            9 => "+992" + phoneNumber.Trim(),
            12 when phoneNumber[0] != '+' => "+" + phoneNumber,
            _ => phoneNumber
        };

        var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber,
            cancellationToken: cancellationToken);
        if (user != null)
        {
            user.PhoneNumberConfirmed = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        return SmsVerificationCodeStatus.CodeVerifySuccess;
    }
}