using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using Newtonsoft.Json;

namespace MRA.Jobs.Infrastructure.Services;
public class JobVacancyHttpClientService : IHttpClientDispatcher<CreateJobVacancyTestCommand, TestInfoDTO, TestPassDTO>
{
    public Task<TestInfoDTO> SendTestCreationRequest(CreateJobVacancyTestCommand request)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var data = new { Name = "John", Age = 30 };

                var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.example.com/post", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Успешный ответ: " + responseContent);
                }
                else
                {
                    Console.WriteLine("Ошибка при выполнении запроса. Код статуса: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }
    }

    public Task<TestPassDTO> SendTestPassRequest(TestPassDTO request)
    {
        throw new NotImplementedException();
    }
}
