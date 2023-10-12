using System.Text.Json.Serialization;

namespace MRA.Identity.Application.Contract.User.Responses;

public class JwtTokenResponse
{
    [JsonPropertyName("AccessToken")]
    public string AccessToken { get; set; }
    [JsonPropertyName("RefreshToken")]
    public string RefreshToken { get; set; }
    public DateTime AccessTokenValidTo { get; set; }
}