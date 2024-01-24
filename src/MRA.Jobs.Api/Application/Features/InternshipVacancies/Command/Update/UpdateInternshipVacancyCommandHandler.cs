using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Command.Update;

public class UpdateInternshipVacancyCommandHandler : IRequestHandler<UpdateInternshipVacancyCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly IMapper _mapper;
    private readonly ISlugGeneratorService _slugService;

    public UpdateInternshipVacancyCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService, ISlugGeneratorService slugService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
    }

    public async Task<string> Handle(UpdateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        //var internship = await _context.Internships.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
        //_ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Id);

        //var category = await _context.Categories.FindAsync(new object[] { request.CategoryId }, cancellationToken: cancellationToken);
        //_ = category ?? throw new NotFoundException(nameof(VacancyCategory), request.CategoryId);

        InternshipVacancy internship = await _context.Internships
            .Include(i => i.Category)
            .Include(i => i.VacancyQuestions)
            .Include(i => i.VacancyTasks)
            .FirstOrDefaultAsync(i => i.Slug == request.Slug, cancellationToken);
        _ = internship ?? throw new NotFoundException(nameof(InternshipVacancy), request.Slug);

        _mapper.Map(request, internship);

        VacancyTimelineEvent timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = internship.Id,
            EventType = TimelineEventType.Updated,
            Time = _dateTime.Now,
            Note = "Internship updated",
            CreateBy = _currentUserService.GetUserName() ?? string.Empty
        };
        await _context.VacancyTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return internship.Slug;
    }
}