using MockTestingApi.Entities;
using Newtonsoft.Json;

namespace MockTestingApi.Services;

public class TestService : ITestService
{
    private readonly HttpClient _http;

    public TestService(HttpClient http)
    {
        _http = http;
    }

    public CreateTestResponse CreateTest(CreateTestRequest request)
    {
        return new CreateTestResponse
        {
            TestId = Guid.NewGuid(),
            MaxScore = new Random().Next(1, 100)
        };
    }

    public async Task PassTest(PassTestRequest request)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:5001/api/", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Success: " + responseContent);
                }
                else
                {
                    Console.WriteLine("Request error. Status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception was thrown: " + ex.Message);
            }
        }
    }
}
