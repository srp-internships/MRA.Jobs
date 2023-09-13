using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class SmsVerificationCodeCheckQueryHandler : IRequestHandler<SmsVerificationCodeCheckQuery, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public SmsVerificationCodeCheckQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<bool> Handle(SmsVerificationCodeCheckQuery request, CancellationToken cancellationToken)
    {
        var expirationTime = DateTime.Now.AddMinutes(-1);

        var result = await _context.ConfirmationCodes
            .FirstOrDefaultAsync(c => c.Code == request.Code && c.PhoneNumber == request.PhoneNumber && c.SentAt >= expirationTime);

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

        if ((user is not null && user.PhoneNumberConfirmed == true) || user is null) return false;

        if (result != null)
        {
            user.PhoneNumberConfirmed = true;
            await _userManager.UpdateAsync(user);
            return true;
        }
        return false;

    }
}
