using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancyTest;
public class CreateJobVacancyTestCommandHandler : IRequestHandler<CreateJobVacancyTestCommand, TestInfoDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpClientDispatcher<CreateJobVacancyTestCommand, TestInfoDTO, TestPassDTO> _httpClient;

    public CreateJobVacancyTestCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService,
        IHttpClientDispatcher<CreateJobVacancyTestCommand, TestInfoDTO, TestPassDTO> httpClient)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _httpClient = httpClient;
    }
    public Task<TestInfoDTO> Handle(CreateJobVacancyTestCommand request, CancellationToken cancellationToken)
    {

    }
}
