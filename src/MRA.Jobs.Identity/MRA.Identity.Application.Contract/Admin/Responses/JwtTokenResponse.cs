namespace MRA.Identity.Application.Contract.Admin.Responses;

public class JwtTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenValidTo { get; set; }
    public DateTime AccessTokenValidTo { get; set; }
}