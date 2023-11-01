using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Commands.ResetPassword;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.UserPassword;
public class ResetUserPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public ResetUserPasswordCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.PhoneNumber[0] != '+') request.PhoneNumber = "+" + request.PhoneNumber.Trim();
        var expirationTime = DateTime.Now.AddMinutes(-30);

        var codeConfirm = _context.ConfirmationCodes
            .Any(c => c.Code == request.Code && c.PhoneNumber == request.PhoneNumber && c.SentAt >= expirationTime);

        if (!codeConfirm) return false;

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (user == null)
            throw new NotFoundException("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (!result.Succeeded)
            return false;

        return true;
    }
}
