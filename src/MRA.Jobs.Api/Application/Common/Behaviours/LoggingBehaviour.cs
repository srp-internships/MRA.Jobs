using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MRA.Jobs.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        Guid? userId = _currentUserService.GetUserId();
        string userName = _currentUserService.GetUserName() ?? string.Empty;
        await Task.CompletedTask;

        _logger.LogInformation("MRA.Jobs Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}