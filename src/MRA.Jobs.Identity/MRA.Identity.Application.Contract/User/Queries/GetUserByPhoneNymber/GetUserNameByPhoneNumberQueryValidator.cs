using FluentValidation;


namespace MRA.Identity.Application.Contract.User.Queries.GetUserByPhoneNymber;
public class GetUserNameByPhoneNumberQueryValidator : AbstractValidator<GetUserNameByPhoneNumberQuery>
{
    public GetUserNameByPhoneNumberQueryValidator()
    {
        RuleFor(x=>x.PhoneNumber).NotEmpty();
        RuleFor(p => p.PhoneNumber).Matches(@"^\+992\d{9}$")
            .WithMessage("Invalid phone number. Example : +992921234567");
    }
}
