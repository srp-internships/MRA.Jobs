using System.Text.Json;
using System.Text;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.ApplicationServices;
public class TaskService:ITaskService
{
    private readonly HttpClient _httpClient = new();
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public TaskService(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<VacancyTaskDetail>> GetGetVacancyTaskDetailAsync(CreateApplicationCommand application, CancellationToken cancellationToken)
    {
        var applicantId = _currentUserService.GetUserId() ?? Guid.Empty;
        _httpClient.DefaultRequestHeaders.Add("API_KEY", "123");
        var taskDetails = new List<VacancyTaskDetail>();

        foreach (var tResponses in application.TaskResponses)
        {
            var taskDetail = new VacancyTaskDetail
            {
                ApplicantId= applicantId,
                Codes = tResponses.Code,
                TaskId = tResponses.TaskId,
            };

            var task = _context.VacancyTasks.Where(v => v.Id == tResponses.TaskId);
            foreach (var vTasks in task)
            {
                var r = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7046/api/CodeAnalyzer/Analyze"),
                    Content = new StringContent(
                        $@"{{
                ""codes"": [
                    ""{vTasks.Test}"",
                    ""{tResponses.Code}""
                ],
                ""dotNetVersionInfo"": {{
                    ""language"": ""CSharp"",
                    ""version"": ""NET6""
                }}
            }}", Encoding.UTF8, "application/json")};

                try
                {
                    using var response = await _httpClient.SendAsync(r, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    var jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
                    bool success = jsonDocument.RootElement.GetProperty("success").GetBoolean();
                    taskDetail.Success = success ? TaskSuccsess.Success : TaskSuccsess.Failure;
                }
                catch (Exception ex)
                {
                    taskDetail.Success = TaskSuccsess.Error;
                    taskDetail.Log = $"Something went wrong! Message={ex.Message},Data = {DateTime.Now} ";
                }
            }
            taskDetails.Add(taskDetail);
        }
        return taskDetails;
    }
}
