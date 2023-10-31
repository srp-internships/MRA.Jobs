using System.Text.Json.Serialization;

namespace MRA.Identity.Application.Features.Users.Command.GoogleAuth;

#nullable disable
public class GoogleResponseModel
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    [JsonPropertyName("id_token")] public string TokenId { get; set; }
    [JsonPropertyName("scope")] public string Scope { get; set; }
    [JsonPropertyName("token_type")] public string TokenType { get; set; }
}