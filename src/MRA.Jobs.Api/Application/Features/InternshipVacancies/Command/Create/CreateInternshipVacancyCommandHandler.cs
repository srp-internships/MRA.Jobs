using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using IEmailService = MRA.Configurations.Common.Interfaces.Services.IEmailService;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Create;

public class CreateInternshipVacancyCommandHandler : IRequestHandler<CreateInternshipVacancyCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;

    public CreateInternshipVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService, ISlugGeneratorService slugService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<string> Handle(CreateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        VacancyCategory category = await _context.Categories.FindAsync(request.CategoryId);
        _ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        var internship = _mapper.Map<InternshipVacancy>(request);

        internship.Slug = GenerateSlug(internship);
        internship.CreatedByEmail = _currentUserService.GetEmail();
        internship.LastModifiedAt = internship.CreatedAt = _dateTime.Now;
        internship.LastModifiedBy = internship.CreatedBy = _currentUserService.GetUserId() ?? Guid.Empty;

        await _context.Internships.AddAsync(internship, cancellationToken);

        VacancyTimelineEvent timelineEvent = new()
        {
            VacancyId = internship.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Internship created",
            CreateBy = _currentUserService.GetUserName() ?? string.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return internship.Slug;
    }

    private string GenerateSlug(InternshipVacancy internship) =>
        _slugService.GenerateSlug($"{internship.Title}-{internship.PublishDate.Year}-{internship.PublishDate.Month}");
}