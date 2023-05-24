using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Verifications;

public class ChangePhoneNumberCommandHandler : IRequestHandler<ChangePhoneNumberCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePhoneNumberCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        if (_userManager.Users.Any(u => u.NormalizedEmail == request.NewPhoneNumber && u.Id != u.Id))
            throw new ValidationException(new[] { new ValidationFailure() { PropertyName = nameof(request.NewPhoneNumber), ErrorMessage = $"Account with {request.NewPhoneNumber} phone number already exist!" } });

        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.NewPhoneNumber);

        //TODO: send sms
        // await smsSender.SendSmsAsync(newPhoneNumber, $"Your verification code is: {token}");.
        Console.WriteLine($"Confirmation link: {token}");
        return Unit.Value;
    }
}
