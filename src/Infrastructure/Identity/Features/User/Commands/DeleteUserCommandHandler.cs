//using Microsoft.AspNetCore.Identity;

//namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

//public class DeleteUserCommandHandler : IRequestHandler<DeleteEntityCommand<ApplicationUser>, Unit>
//{
//    private readonly UserManager<ApplicationUser> _userManager;

//    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
//    {
//        _userManager = userManager;
//    }

//    public async Task<Unit> Handle(DeleteEntityCommand<ApplicationUser> request, CancellationToken cancellationToken)
//    {
//        var user = await _userManager.FindByIdAsync(request.Id.ToString());
//        if (user == null)
//            throw new EntityNotFoundException(nameof(ApplicationUser), request.Id);

//        var result = await _userManager.DeleteAsync(user);
//        if (!result.Succeeded)
//            throw new BusinessLogicException($"User cannot be deleted because: {string.Join('\n', result.Errors.Select(r => r.Description))}");

//        return Unit.Value;
//    }
//}

