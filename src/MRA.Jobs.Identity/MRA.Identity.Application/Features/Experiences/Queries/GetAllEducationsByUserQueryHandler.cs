using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Experiences.Query;
using MRA.Identity.Application.Contract.Experiences.Responses;

namespace MRA.Identity.Application.Features.Educations.Queries;

public class GetAllExperiencesByUserQueryHandler : IRequestHandler<GetExperiencesByUserQuery, List<UserExperienceResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IMapper _mapper;

    public GetAllExperiencesByUserQueryHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor, IMapper mapper)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
        _mapper = mapper;
    }

    public async Task<List<UserExperienceResponse>> Handle(GetExperiencesByUserQuery request,
        CancellationToken cancellationToken)
    {
        var roles = _userHttpContextAccessor.GetUserRoles();
        var userName = _userHttpContextAccessor.GetUserName();
        if (request.UserName != null && roles.Any(role => role == "Applicant") && userName != request.UserName)
            throw new ForbiddenAccessException("Access is denied");

        if (request.UserName != null)
            userName = request.UserName;

        var user = await _context.Users
            .Include(u => u.Experiences)
            .FirstOrDefaultAsync(u => u.UserName == userName);

        _ = user ?? throw new NotFoundException("user is not found");
        var userExperiencesResponses = user.Experiences
            .Select(e => _mapper.Map<UserExperienceResponse>(e)).ToList();
        return userExperiencesResponses;
    }
}
