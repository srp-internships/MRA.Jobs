
using AutoMapper;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.UserProfiles;
public class UserProfilesProfile : Profile
{
    public UserProfilesProfile()
    {
        CreateMap<UpdateProfileCommand, ApplicationUser>();
    }
}
