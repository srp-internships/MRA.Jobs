using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using MRA.Jobs.Infrastructure.Persistence;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Verifications;

public class ConfirmEmailChangeCommandHandler : IRequestHandler<ConfirmEmailChangeCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public ConfirmEmailChangeCommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        if (_userManager.Users.Any(u => u.NormalizedEmail == request.NewEmail && u.Id != u.Id))
            throw new ValidationException(new[] { new ValidationFailure() { PropertyName = nameof(request.NewEmail), ErrorMessage = $"Account with {request.NewEmail} email already exist!" } });

        var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, request.Token);
        if (!result.Succeeded)
        {
            throw new ValidationException(new[] { new ValidationFailure() {
                PropertyName = nameof(request.NewEmail),
                ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(e=>e.Description))
            } });
        }

        //TODO: Notify Domain instead
        var domainUser = await _dbContext.Set<Domain.Entities.User>().FindAsync(new object[] { user.Id }, cancellationToken);
        domainUser.Email = request.NewEmail;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
