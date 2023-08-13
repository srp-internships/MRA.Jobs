// using Microsoft.AspNetCore.Identity;
// using MRA.Jobs.Infrastructure.Shared.Users.Queries;
//
// namespace MRA.Jobs.Infrastructure.Identity.Features.User.Queries;
//
// public class EmailExistQueryHandler : IRequestHandler<EmailExistQuery, bool>
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public EmailExistQueryHandler(UserManager<ApplicationUser> userManager)
//     {
//         _userManager = userManager;
//     }
//
//     public async Task<bool> Handle(EmailExistQuery request, CancellationToken cancellationToken)
//     {
//         ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
//         return user is not null;
//     }
// }