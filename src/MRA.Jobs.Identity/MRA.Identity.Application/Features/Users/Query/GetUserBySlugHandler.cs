using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class GetUserBySlugHandler : IRequestHandler<GetUserByUsernameQuery, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GetUserBySlugHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<UserResponse> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
        var result = _mapper.Map<UserResponse>(user);

        return result;
    }
}
