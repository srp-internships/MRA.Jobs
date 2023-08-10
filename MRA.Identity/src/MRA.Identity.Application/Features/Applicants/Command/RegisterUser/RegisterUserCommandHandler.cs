using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Applicants.Command.RegisterUser;

public class RegisterUserCommandHandler:IRequestHandler<RegisterUserCommand,Guid?>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Guid?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // var user = _mapper.Map<ApplicationUser>(request);
        var user = new ApplicationUser
        {
            Id = new Guid(),
            UserName = request.Username,
            NormalizedUserName = request.Username,
            Email = request.Username,
            NormalizedEmail = request.Username,
            EmailConfirmed = true,
            // PasswordHash = null,
            // SecurityStamp = null,
            // ConcurrencyStamp = null,
            // PhoneNumber = null,
            // PhoneNumberConfirmed = false,
            // TwoFactorEnabled = false,
            // LockoutEnd = null,
            // LockoutEnabled = false,
            // AccessFailedCount = 0
        };
        var result = await _userManager.CreateAsync(user,request.Password);
        if (result.Succeeded)
        {
            return user.Id;
        }
        return null;
    }
}