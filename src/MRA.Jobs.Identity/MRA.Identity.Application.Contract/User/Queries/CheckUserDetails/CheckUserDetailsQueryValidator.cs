using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
public class CheckUserDetailsQueryValidator : AbstractValidator<CheckUserDetailsQuery>
{
    public CheckUserDetailsQueryValidator()
    {
        RuleFor(s => s.PhoneNumber).Matches(@"^(?:\d{9}|\+992\d{9}|992\d{9})$").WithMessage("Invalid phone number. Example : +992921234567, 992921234567, 921234567");
        RuleFor(s => s.UserName.Length > 3);
        RuleFor(s => s.Email).EmailAddress();
    }
}
