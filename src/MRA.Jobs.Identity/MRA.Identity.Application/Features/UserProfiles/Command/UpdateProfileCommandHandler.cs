using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserProfiles.Command;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateProfileCommandHandler(UserManager<ApplicationUser> userManager,
        IUserHttpContextAccessor userHttpContextAccessor,
        IMapper mapper)
    {
        _userManager = userManager;
        _userHttpContextAccessor = userHttpContextAccessor;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(_userHttpContextAccessor.GetUserName());
        _ = user ?? throw new NotFoundException("user is not found");
        if (user.PhoneNumber != request.PhoneNumber) user.PhoneNumberConfirmed = false;
        _mapper.Map(request, user);

        return (await _userManager.UpdateAsync(user)).Succeeded;

    }
}
