using Microsoft.Extensions.Configuration;

namespace MRA.Jobs.Application.Features.Users;

public class GetConfigs : IRequest<List<string>>
{
    
}

public class GetConfigsHandler(IConfiguration configuration ) : IRequestHandler<GetConfigs, List<string>>
{
    public Task<List<string>> Handle(GetConfigs request, CancellationToken cancellationToken)
    {
        return Task.FromResult<List<string>>([
            "UserApiUrl: " + configuration["MraJobs-IdentityApi:Users"],
            "applicationId: " + configuration["Application:Id"],
            "applicationSecret: " + configuration["Application:ClientSecret"]
        ]);
    }
}