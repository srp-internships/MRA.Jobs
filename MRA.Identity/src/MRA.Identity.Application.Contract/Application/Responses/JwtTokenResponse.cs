namespace MRA.Identity.Application.Contract.Application.Responses;

public class JwtTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenValidTo { get; set; }
}