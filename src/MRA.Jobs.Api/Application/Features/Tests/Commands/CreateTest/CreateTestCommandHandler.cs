using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands.CreateTest;

namespace MRA.Jobs.Application.Features.Tests.Commands.CreateTest;

public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, TestInfoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private readonly ITestHttpClientService _httpClient;

    public CreateTestCommandHandler(IApplicationDbContext context, IDateTime dateTime,
        ICurrentUserService currentUserService,
        ITestHttpClientService httpClient)
    {
        _context = context;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _httpClient = httpClient;
    }
    public async Task<TestInfoDto> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        TestInfoDto result = await _httpClient.SendTestCreationRequest(request);

        Test test = new Test
        {
            CreatedAt = _dateTime.Now,
            CreatedBy = _currentUserService.GetId() ?? Guid.Empty,
            Description = string.Empty,
            Duration = TimeSpan.Zero,
            Id = result.TestId,
            NumberOfQuestion = request.NumberOfQuestion,
            PassingScore = result.MaxScore,
            Title = "test",
            VacancyId = request.Id
        };

        await _context.Tests.AddAsync(test, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result;
    }
}