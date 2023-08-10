using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Application.Contracts.Identity.Events;
using MRA.Jobs.Infrastructure.Shared.Auth.Commands;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;
using ValidationException = MRA.Jobs.Application.Common.Exceptions.ValidationException;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly string pattern = "^[a-zA-Zа-яА-Я]+$";

    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Must(s => Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.LastName).NotEmpty().Must(s => Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.Patronymic).Must(s => string.IsNullOrEmpty(s) || Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.TermsAccepted).Equal(true);
        RuleFor(x => x.Gender).IsInEnum();

        RuleFor(x => x.Password).NotEmpty().MaximumLength(8);
        RuleFor(x => x.PasswordConfirmation).NotEmpty().Equal(x => x.Password);
        RuleFor(x => x.PhoneVerificationCode).NotEmpty();
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        //TODO: check phone verification code

        var user = new ApplicationUser()
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserName = $"{request.LastName} {request.FirstName} {request.Patronymic}",
            PhoneNumberConfirmed = true,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new ValidationException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

        result = await _userManager.AddToRoleAsync(user, "Applicant");
        if (!result.Succeeded)
            throw new ValidationException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));

        var notification = new NewIdentityRegisteredEvent()
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender
        };
        await _mediator.Publish(notification, cancellationToken);

        return user.Id;
    }
}
