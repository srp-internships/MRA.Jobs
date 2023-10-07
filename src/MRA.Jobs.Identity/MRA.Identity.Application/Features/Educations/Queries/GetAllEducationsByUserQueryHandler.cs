using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Educations.Responses;

namespace MRA.Identity.Application.Features.Educations.Queries;

public class GetAllEducationsByUserQueryHandler : IRequestHandler<GetEducationsByUserQuery,
    ApplicationResponse<List<UserEducationResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IMapper _mapper;

    public GetAllEducationsByUserQueryHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor, IMapper mapper)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ApplicationResponse<List<UserEducationResponse>>> Handle(GetEducationsByUserQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var roles = _userHttpContextAccessor.GetUserRoles();
            var userName = _userHttpContextAccessor.GetUserName();
            if (request.UserName != null && roles.Any(role => role == "Applicant" && userName != request.UserName))
                return new ApplicationResponseBuilder<List<UserEducationResponse>>()
                    .SetErrorMessage("Access is denied")
                    .Success(false).Build();

            var user = await _context.Users
                .Include(u => u.Educations)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return new ApplicationResponseBuilder<List<UserEducationResponse>>()
                    .SetErrorMessage("user not found")
                    .Success(false).Build();
            var userEducationResponses = user.Educations
                .Select(e => _mapper.Map<UserEducationResponse>(e)).ToList();
           
            return new ApplicationResponseBuilder<List<UserEducationResponse>>()
                .SetResponse(userEducationResponses).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<List<UserEducationResponse>>().SetException(e).Success(false).Build();
        }
    }
}
