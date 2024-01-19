using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MRA.Identity.Application.Contract.Messages.Commands;
public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(sm => sm.Text).NotEmpty();
        RuleFor(sm => sm.Phone).NotEmpty().Matches(@"^(?:\d{9}|\+992\d{9}|992\d{9})$").WithMessage("Invalid phone number. Example : +992921234567, 992921234567, 921234567");
    }
}
