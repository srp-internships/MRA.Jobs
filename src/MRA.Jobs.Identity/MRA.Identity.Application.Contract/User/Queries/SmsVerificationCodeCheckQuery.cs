using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries;
public class SmsVerificationCodeCheckQuery : IRequest<SmsVerificationCodeStatus>
{
    public string PhoneNumber { get; set; }
    public int Code { get; set; }
}

public enum SmsVerificationCodeStatus
{
    
    CodeVerifySuccess,
    CodeVerifyFailure,
    CodeVerifySuccess_ButUserDontSignUp,
}
