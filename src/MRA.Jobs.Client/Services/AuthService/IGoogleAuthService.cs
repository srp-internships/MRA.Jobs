
namespace MRA.Jobs.Client.Services.AuthService;

public interface IGoogleAuthService
{
    public Task<string> GetToken();

}

