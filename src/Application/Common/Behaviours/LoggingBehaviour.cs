using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MRA.Jobs.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        Guid? userId = _currentUserService.GetId();
        string userName = _currentUserService.GetUserName() ?? "";
        await Task.CompletedTask;

        _logger.LogInformation("MRA.Jobs Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}