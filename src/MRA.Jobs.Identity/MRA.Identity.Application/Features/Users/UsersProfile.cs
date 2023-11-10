using AutoMapper;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users;
public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<ApplicationUser, UserResponse>()
            .ForMember(dest => dest.FullName, opt => opt
            .MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
