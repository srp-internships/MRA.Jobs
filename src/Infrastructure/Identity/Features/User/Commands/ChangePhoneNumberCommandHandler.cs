using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

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
        // await smsSender.SendSmsAsync(newPhoneNumber, $"Your verification code is: {token}");
        return Unit.Value;
    }
}

public class ConfirmPhoneNumberChangeCommandHandler : IRequestHandler<ConfirmPhoneNumberChangeCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public ConfirmPhoneNumberChangeCommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ConfirmPhoneNumberChangeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        if (_userManager.Users.Any(u => u.NormalizedEmail == request.NewPhoneNumber && u.Id != u.Id))
            throw new ValidationException(new[] { new ValidationFailure() { PropertyName = nameof(request.NewPhoneNumber), ErrorMessage = $"Account with {request.NewPhoneNumber} phone number already exist!" } });

        var result = await _userManager.ChangePhoneNumberAsync(user, request.NewPhoneNumber, request.Token);
        if (!result.Succeeded)
        {
            throw new ValidationException(new[] { new ValidationFailure() {
                PropertyName = nameof(request.NewPhoneNumber),
                ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e=>e.Description))
            } });
        }

        //TODO: Notify Domain instead
        var domainUser = await _dbContext.Set<Domain.Entities.User>().FirstAsync(u => u.IdentityId == user.Id, cancellationToken);
        domainUser.PhoneNumber = request.NewPhoneNumber;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
