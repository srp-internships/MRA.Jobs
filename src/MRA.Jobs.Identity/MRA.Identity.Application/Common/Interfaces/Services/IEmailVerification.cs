using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IEmailVerification
{
    Task<bool> SendVerificationEmailAsync(ApplicationUser user);
    Task VerifyEmailAsync(string token);
}
