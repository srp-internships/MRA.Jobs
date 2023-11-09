using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;

namespace MRA.Identity.Application.Features.Experiences.Commands;
public class UpdateExperienceDetailCommandHandler : IRequestHandler<UpdateExperienceDetailCommand, Guid>
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
    public async Task<Guid> Handle(UpdateExperienceDetailCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContextAccessor.GetUserId();
        var user = await _context.Users
            .Include(u => u.Experiences)
            .FirstOrDefaultAsync(u => u.Id == userId);
        _ = user ?? throw new NotFoundException("user is not found");

        var experienceDetail = user.Experiences.FirstOrDefault(e => e.Id == request.Id);
        _ = experienceDetail ?? throw new NotFoundException("experience not exits");

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        var jobTitle = textInfo.ToTitleCase(request.JobTitle.Trim());
        var company = textInfo.ToTitleCase(request.CompanyName.Trim());
        request.Description = textInfo.ToTitleCase(request.Description.Trim());
        request.Address = request.Address.Trim();

        var existingExperience = user.Experiences.FirstOrDefault(e =>
            e.Id != request.Id &&
            e.JobTitle.Equals(jobTitle, StringComparison.OrdinalIgnoreCase) &&
            e.CompanyName.Equals(company, StringComparison.OrdinalIgnoreCase));

        if (existingExperience != null)
        {
            throw new DuplicateWaitObjectException("Experience detail already exists.");
        }

        _mapper.Map(request, experienceDetail);
        experienceDetail.JobTitle = jobTitle;
        experienceDetail.CompanyName = company;

        await _context.SaveChangesAsync();
        return experienceDetail.Id;
    }
}
