using MediatR;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Application.Features.Users.Query;

public class
    SmsVerificationCodeCheckQueryHandler : IRequestHandler<SmsVerificationCodeCheckQuery, SmsVerificationCodeStatus>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsCodeChecker _codeChecker;

    public SmsVerificationCodeCheckQueryHandler(IApplicationDbContext context, ISmsCodeChecker codeChecker)
    {
        _context = context;
        _codeChecker = codeChecker;
    }

    public Task<SmsVerificationCodeStatus> Handle(SmsVerificationCodeCheckQuery request,
        CancellationToken cancellationToken)
    {

        bool result = _codeChecker.VerifyPhone(request.Code, request.PhoneNumber);

        if (!result)
            return Task.FromResult(SmsVerificationCodeStatus.CodeVerifyFailure);

        return Task.FromResult(SmsVerificationCodeStatus.CodeVerifySuccess);
    }
}