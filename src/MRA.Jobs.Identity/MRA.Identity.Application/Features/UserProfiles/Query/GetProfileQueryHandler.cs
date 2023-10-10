using System.Data;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Identity.Application.Features.UserProfiles.Query;
public class GetProfileQueryHandler : IRequestHandler<GetPofileQuery, ApplicationResponse<UserProfileResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public GetProfileQueryHandler(IApplicationDbContext context,
        IMapper mapper, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<ApplicationResponse<UserProfileResponse>> Handle(GetPofileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userRoles = _userHttpContextAccessor.GetUserRoles();
            var userName = _userHttpContextAccessor.GetUserName();

            if (request.UserName != null && userRoles.Any(role => role == "Applicant") && userName != request.UserName)
                return new ApplicationResponseBuilder<UserProfileResponse>()
                    .SetErrorMessage("Access is denied").Success(false).Build();

            if (request.UserName != null)
                userName = request.UserName;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return new ApplicationResponseBuilder<UserProfileResponse>()
                    .SetErrorMessage("User not found").Success(false).Build();

            var response = _mapper.Map<UserProfileResponse>(user);
            return new ApplicationResponseBuilder<UserProfileResponse>().SetResponse(response).Build();
        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<UserProfileResponse>()
                .SetException(ex).Success(false).Build();
        }
    }
}
