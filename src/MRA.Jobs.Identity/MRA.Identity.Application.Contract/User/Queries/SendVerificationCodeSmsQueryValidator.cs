using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MRA.Identity.Application.Contract.User.Queries;
public class SendVerificationCodeSmsQueryValidator : AbstractValidator<SendVerificationCodeSmsQuery>
{
    public SendVerificationCodeSmsQueryValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(d{9}|992\d{9}|\+992\d{9})$")
            .WithMessage("Invalid phone number format.");
    }
}
