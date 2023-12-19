using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Application.Common.Behaviours;
public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public LoggingBehaviour(ILogger<TRequest> logger, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _logger = logger;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        Guid? userId = _userHttpContextAccessor.GetUserId();
        string userName = _userHttpContextAccessor.GetUserName() ?? "";
        await Task.CompletedTask;

        _logger.LogInformation("MRA.Jobs Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}
