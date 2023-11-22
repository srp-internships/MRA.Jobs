using System.Text;
using System.Text.Json;
using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.ApplicationServices;

public class VacancyTasksService(IApplicationDbContext _context) : IVacancyTaskService
{
    private readonly HttpClient _httpClient = new();

    public async Task CheckVacancyTasksAsync(Guid applicationId, IEnumerable<TaskResponseDto> taskResponses,
        CancellationToken cancellationToken)
    {
        _httpClient.DefaultRequestHeaders.Add("API_KEY", "123");

        foreach (var taskResponse in taskResponses)
        {
            var vacancyTaskDetail = new VacancyTaskDetail
            {
                ApplicantId = applicationId,
                Codes = taskResponse.Code,
                TaskId = taskResponse.TaskId,
            };
            var vacancyTasks = _context.VacancyTasks.Where(v => v.Id == taskResponse.TaskId);
            foreach (var vacancyTask in vacancyTasks)
            {
                var r = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7046/api/CodeAnalyzer/Analyze"),
                    Content = new StringContent(
                        $@"{{
                    ""codes"": [
                        ""{vacancyTask.Test}"",
                        ""{taskResponse.Code}""
                    ],
                    ""dotNetVersionInfo"": {{
                        ""language"": ""CSharp"",
                        ""version"": ""NET6""
                    }}
                }}", Encoding.UTF8, "application/json")
                };

                try
                {
                    using var response = await _httpClient.SendAsync(r, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    var jsonDocument = JsonDocument.Parse(responseBody);
                    var success = jsonDocument.RootElement.GetProperty("success").GetBoolean();
                    vacancyTaskDetail.Success = success ? TaskSuccsess.Success : TaskSuccsess.Failure;
                }
                catch (Exception ex)
                {
                    vacancyTaskDetail.Success = TaskSuccsess.Error;
                    vacancyTaskDetail.Log = $"Something went wrong! Message={ex.Message}, Data = {DateTime.Now} ";
                }
            }

            await _context.VacancyTaskDetails.AddAsync(vacancyTaskDetail, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}