using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Experiences.Commands;
public class CreateExperienceDetailCommandHandler : IRequestHandler<CreateExperienceDetailCommand, ApplicationResponse<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IMapper _mapper;

    public CreateExperienceDetailCommandHandler(IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor,
        IMapper mapper)
    {
        _context = context;
        _userHttpContextAccessor = userHttpContextAccessor;
        _mapper = mapper;
    }
    public async Task<ApplicationResponse<Guid>> Handle(CreateExperienceDetailCommand request, CancellationToken cancellationToken)
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

            var experienceDetail = _mapper.Map<ExperienceDetail>(request);

            user.Experiences.Add(experienceDetail);

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
