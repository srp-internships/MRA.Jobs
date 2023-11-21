using MediatR;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.CV;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Experiences.Query;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Application.Services.GeneratePdfCV;
using QuestPDF.Fluent;


namespace MRA.Identity.Application.Features.CV;
public class CVGenerateQueryHandler : IRequestHandler<CVGenerateQuery, MemoryStream>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserHttpContextAccessor _userHttpContext;
    private readonly IMediator _mediator;

    public CVGenerateQueryHandler(IApplicationDbContext dbContext,
        IUserHttpContextAccessor userHttpContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _userHttpContext = userHttpContext;
        _mediator = mediator;
    }

    public async Task<MemoryStream> Handle(CVGenerateQuery request, CancellationToken cancellationToken)
    {
        var userProfile = await _mediator.Send(new GetPofileQuery());
        var userSkills = await _mediator.Send(new GetUserSkillsQuery());
        var userEducations = await _mediator.Send(new GetEducationsByUserQuery());
        var userExperience = await _mediator.Send(new GetExperiencesByUserQuery());


        InvoiceDocument document = new InvoiceDocument(userProfile, userSkills,
            userEducations, userExperience);

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
        return stream;

    }


}