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

// {
// "access_token": "ya29.a0AfB_byBTKxJPiSWC8OuR6A1BhiK-22068oqXorX3nXpBSLGXwa1YLwxpdePL0Bsqf9QGGolt6_HUyHNFqIGxAhAJBlaxENV_HiVje1usEniP4NETP6WSztoIZlwMOu0PGoM4k-Cfi4Pw3J1b8_-jPzMjnXP7MVOt8QaCgYKAZoSARMSFQGOcNnC26DEORdHHpwiJNdPCXY1NA0169",
// "expires_in": 3595,
// "scope": "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email openid",
// "token_type": "Bearer",
// "id_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImEwNmFmMGI2OGEyMTE5ZDY5MmNhYzRhYmY0MTVmZjM3ODgxMzZmNjUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiI0NTI0MjI1NDEwODktcTJpbGJxZGhxczE4YmJuMzU2cWtrYms3YmNxaWhva2EuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI0NTI0MjI1NDEwODktcTJpbGJxZGhxczE4YmJuMzU2cWtrYms3YmNxaWhva2EuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMTcwNTgxMTMxMzMyNDIzMTUyNTQiLCJlbWFpbCI6ImZpcnV6aXNvYm9ldkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6Ik9Kb3dSUXA4djVHc0ItNUIyZmFlX0EiLCJuYW1lIjoiSVJGIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FDZzhvY0t4UFR6ZlR1czNuS1dvR2wzVG96QS1ERzVtOGVzVEtFcGRVNjVKMXEwMz1zOTYtYyIsImdpdmVuX25hbWUiOiJJUkYiLCJsb2NhbGUiOiJydSIsImlhdCI6MTY5ODM5MDAyNiwiZXhwIjoxNjk4MzkzNjI2fQ.jJn-txbyM00KtmTJS6JGgM-tY328e6nmFxYcX1RIJqgLr0GOTMiEuP5495qXurUDGyXeu2LjUfgarArPHMbdkZpK-43isQvELvzUN62dZdoYW1Hhyh4XXxmSpsVWAGCgYGV4yGYtnb0--FDKQA3pGDBZbsK4YBxxLPDTStrQ_OyiqykvRTvR4dQM9qEXYMhvTY4U7LTOVByUiVHFeDlS2OAekRhYhBp6M4lDyvr-RI-rnbXBd4f0fbJdlNcdy7u9UHzZ6SnjQz_V3b70T3CVnYB6rU9t8vrDSDHuVY9Uc-oEN4hnh1LI_oDw_zoHW6X_4A0H2IO9Z_1eWB0O3cG-0A"
//
// }