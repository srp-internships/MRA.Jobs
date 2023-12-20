using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MRA.Jobs.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService, IConfiguration configuration)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _currentUserService = currentUserService;
        _configuration = configuration;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;
        int threshold = _configuration.GetValue<int>("LongRunningRequestThreshold");
        if (elapsedMilliseconds > threshold)
        {
            string requestName = typeof(TRequest).Name;
            Guid? userId = _currentUserService.GetUserId();
            string userName = _currentUserService.GetUserName();
            _logger.LogError(
                "MRA.Jobs Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {UserId} {UserName} {Request}",
                requestName, elapsedMilliseconds, userId, userName, request);
        }

        return response;
    }
}