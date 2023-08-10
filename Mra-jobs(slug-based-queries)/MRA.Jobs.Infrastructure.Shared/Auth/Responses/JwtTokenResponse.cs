namespace MRA.Jobs.Infrastructure.Shared.Auth.Responses;

public class JwtTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenValidTo { get; set; }
}
