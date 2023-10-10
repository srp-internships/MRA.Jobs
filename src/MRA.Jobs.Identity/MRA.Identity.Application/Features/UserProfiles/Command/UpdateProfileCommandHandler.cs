using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserProfiles.Command;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ApplicationResponse<bool>>
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
    public async Task<ApplicationResponse<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(_userHttpContextAccessor.GetUserName());

            if (user == null)
            {
                return new ApplicationResponseBuilder<bool>().SetErrorMessage("User not found").Success(false).Build();
            }

            _mapper.Map(request, user);

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded
                     ? new ApplicationResponseBuilder<bool>().SetResponse(true).Build()
                     : new ApplicationResponseBuilder<bool>().SetErrorMessage("Failed to update user.").Success(false).Build();


        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<bool>().SetException(ex).Success(false).Build();
        }


    }
}
