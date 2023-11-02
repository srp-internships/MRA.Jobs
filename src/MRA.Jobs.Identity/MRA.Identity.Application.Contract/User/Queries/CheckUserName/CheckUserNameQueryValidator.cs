using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Queries.CheckUserName;
public class CheckUserNameQueryValidator : AbstractValidator<CheckUserNameQuery>
{
    public CheckUserNameQueryValidator()
    {
        RuleFor(x => x.UserName).NotEmpty()
            .Matches(@"^[a-zA-Z0-9_]*$").WithMessage("UserName can only contain letters and numbers, and underscores")
            .Length(6, 15).WithMessage("UserName must be between 6 and 15 characters"); ;
    }
}
