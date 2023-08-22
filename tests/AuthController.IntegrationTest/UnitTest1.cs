using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace AuthController.IntegrationTest;

[TestFixture]
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    public IntegrationTests(WebApplicationFactory<Program> factoryS)
    {
        //  factory = factoryS;
    }

    [Test]
    public async Task Login_ReturnsJwtToken()
    {
        // Создаем клиента для отправки запросов
        var client = _factory.CreateClient();
        

        // Отправляем POST-запрос на эндпоинт "/login"
        var response = await client.PostAsJsonAsync("/api/auth/login", command);

        // Проверяем, что запрос был успешным
        response.EnsureSuccessStatusCode();

        // Читаем содержимое ответа
        var tokenResponse = await response.Content.ReadFromJsonAsync<JwtTokenResponse>();

        // Проверяем, что полученный токен не пустой
        Assert.NotNull(tokenResponse.AccessToken);
    }
}