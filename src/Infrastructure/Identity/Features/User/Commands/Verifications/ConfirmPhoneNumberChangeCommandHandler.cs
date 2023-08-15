using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Verifications;

public class ConfirmPhoneNumberChangeCommandHandler : IRequestHandler<ConfirmPhoneNumberChangeCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmPhoneNumberChangeCommandHandler(UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ConfirmPhoneNumberChangeCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

        if (_userManager.Users.Any(u => u.NormalizedEmail == request.NewPhoneNumber && u.Id != u.Id))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = nameof(request.NewPhoneNumber),
                    ErrorMessage = $"Account with {request.NewPhoneNumber} phone number already exist!"
                }
            });
        }

        IdentityResult result = await _userManager.ChangePhoneNumberAsync(user, request.NewPhoneNumber, request.Code);
        if (!result.Succeeded)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = nameof(request.NewPhoneNumber),
                    ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description))
                }
            });
        }

        //TODO: Notify Domain instead
        Domain.Entities.User domainUser = await _dbContext.Set<Domain.Entities.User>()
            .FindAsync(new object[] { user.Id }, cancellationToken);
        domainUser.PhoneNumber = request.NewPhoneNumber;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}