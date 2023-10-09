using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;

namespace MRA.Identity.Application.Features.Experiences.Commands;
public class UpdateExperienceDetailCommandHandler : IRequestHandler<UpdateExperienceDetailCommand, ApplicationResponse<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateExperienceDetailCommandHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor,
        IMapper mapper)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
        _mapper = mapper;
    }
    public async Task<ApplicationResponse<Guid>> Handle(UpdateExperienceDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userHttpContextAccessor.GetUserId();
            var user = await _context.Users
                    .Include(u => u.Experiences)
                    .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new ApplicationResponseBuilder<Guid>()
                  .SetErrorMessage("User not found")
                  .Success(false).Build();
            }

            var experienceDetail = user.Experiences.FirstOrDefault(e => e.Id == request.Id);
            if (experienceDetail == null)
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage("experience not exits")
                    .Success(false).Build();

            _mapper.Map(request, experienceDetail);

            await _context.SaveChangesAsync();
            return new ApplicationResponseBuilder<Guid>()
                .SetResponse(experienceDetail.Id).Build();

        }
        catch (Exception ex)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(ex).Success(false).Build();
        }
    }
}
