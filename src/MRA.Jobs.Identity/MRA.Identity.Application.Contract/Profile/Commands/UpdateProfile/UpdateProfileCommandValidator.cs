using FluentValidation;

namespace MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(p => p.Email).EmailAddress();
        RuleFor(p => p.FirstName).NotEmpty();
        RuleFor(p => p.LastName).NotEmpty();
        RuleFor(p => p.PhoneNumber).Matches(@"^\+992\d{9}$")
            .WithMessage("Invalid phone number. Example : +992921234567");
        RuleFor(p => p.DateOfBirth).NotEmpty();
    }
}
