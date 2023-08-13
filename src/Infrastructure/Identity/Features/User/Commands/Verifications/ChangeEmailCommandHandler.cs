// using FluentValidation.Results;
// using Microsoft.AspNetCore.Identity;
// using MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;
//
// namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Verifications;
//
// public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, Unit>
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public ChangeEmailCommandHandler(UserManager<ApplicationUser> userManager)
//     {
//         _userManager = userManager;
//     }
//
//     public async Task<Unit> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
//     {
//         ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString());
//         if (user == null)
//         {
//             throw new NotFoundException(nameof(ApplicationUser), request.UserId);
//         }
//
//         if (user.Email.Equals(request.NewEmail))
//         {
//             return await Task.FromResult(Unit.Value);
//         }
//
//         if (_userManager.Users.Any(u => u.NormalizedEmail == request.NewEmail && u.Id != u.Id))
//         {
//             throw new ValidationException(new[]
//             {
//                 new ValidationFailure
//                 {
//                     PropertyName = nameof(request.NewEmail),
//                     ErrorMessage = $"Account with {request.NewEmail} email already exist!"
//                 }
//             });
//         }
//
//         string token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
//
//         //TODO: Send email to confirm email
//         //var confirmationLink = Path.Combine(.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token, newEmail }, Request.Scheme);
//         //await _emailSender.SendEmailAsync(newEmail, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.");
//         Console.WriteLine($"Confirmation link: {token}");
//
//         return Unit.Value;
//     }
// }