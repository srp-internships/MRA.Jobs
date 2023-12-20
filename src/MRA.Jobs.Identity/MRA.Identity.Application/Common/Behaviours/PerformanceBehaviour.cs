using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MRA.Identity.Application.Common.Interfaces.Services;
using System.Diagnostics;

namespace MRA.Identity.Application.Common.Behaviours;
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IUserHttpContextAccessor userHttpContextAccessor, IConfiguration configuration)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _userHttpContextAccessor = userHttpContextAccessor;
        _configuration = configuration;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken )
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;
        int threshold = _configuration.GetValue<int>("LongRunningRequestThreshold");
        if (elapsedMilliseconds > threshold)
        {
            string requestName = typeof(TRequest).Name;
            Guid? userId = _userHttpContextAccessor.GetUserId();
            string userName = _userHttpContextAccessor.GetUserName();
            _logger.LogError(
                "MRA.Identity Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {UserId} {UserName} {Request}",
                requestName, elapsedMilliseconds, userId, userName, request);
        }

        return response;
    }
}
