using ValidationException = MRA.Jobs.Application.Common.Exceptions.ValidationException;

namespace MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using Common.Interfaces;
using Common.Security;
using Common.SlugGeneratorService;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISlugGeneratorService _slugService;
    private readonly MRA.Configurations.Common.Interfaces.Services.IEmailService _emailService;
    private readonly IHtmlService _htmlService;
    static readonly HttpClient httpClient = new HttpClient();
    public CreateApplicationCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService, ISlugGeneratorService slugService,
        MRA.Configurations.Common.Interfaces.Services.IEmailService emailService, IHtmlService htmlService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
        _slugService = slugService;
        _emailService = emailService;
        _htmlService = htmlService;
    }

    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("API_KEY", "123");
        httpClient.DefaultRequestHeaders.Add("API_KEY", "123");
        Vacancy vacancy = await _context.Vacancies.FindAsync(request.VacancyId);
        _ = vacancy ?? throw new NotFoundException(nameof(Vacancy), request.VacancyId);
        var application = _mapper.Map<Application>(request);

        application.Slug = GenerateSlug(_currentUserService.GetUserName(), vacancy);

        if (await ApplicationExits(application.Slug))
            throw new ConflictException("Duplicate Apply. You have already submitted your application!");

        application.ApplicantId = _currentUserService.GetUserId() ?? Guid.Empty;
        application.ApplicantUsername = _currentUserService.GetUserName() ?? string.Empty;
        await _context.Applications.AddAsync(application, cancellationToken);
        foreach (var tResponses in application.TaskResponses)
        {
            var s = new VacancyTaskDetail
            {
                ApplicantId = application.Id,
                Codes = tResponses.Code,
                TaskId = tResponses.TaksId,
            };
            var task = _context.VacancyTasks.Where(v => v.Id == tResponses.TaksId);
            foreach (var VTasks in task)
            {
                var r = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7046/api/CodeAnalyzer/Analyze"),
                    Content = new StringContent(
                $@"{{
                    ""codes"": [
                        ""{VTasks.Test}"",
                        ""{tResponses.Code}""
                    ],
                    ""dotNetVersionInfo"": {{
                        ""language"": ""CSharp"",
                        ""version"": ""NET6""
                    }}
                }}", Encoding.UTF8, "application/json")
                };

                try
                {
                    using var response = await httpClient.SendAsync(r);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    var jsonDocument = JsonDocument.Parse(responseBody);
                    bool success = jsonDocument.RootElement.GetProperty("success").GetBoolean();
                    s.Success = success ? TaskSuccsess.Success : TaskSuccsess.Failure;
                }
                catch (Exception ex)
                {
                    s.Success = TaskSuccsess.Error;
                    s.Log = $"Something went wrong! Message={ex.Message},Data = {DateTime.Now} ";
                }
            }
            await _context.VacancyTaskDetails.AddAsync(s, cancellationToken);
        }
        ApplicationTimelineEvent timelineEvent = new()
        {
            ApplicationId = application.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTime.Now,
            Note = "Job vacancy created",
            CreateBy = _currentUserService.GetUserId() ?? Guid.Empty
        };

        await _context.ApplicationTimelineEvents.AddAsync(timelineEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _emailService.SendEmailAsync(new[] { vacancy.CreatedByEmail },
            _htmlService.GenerateApplyVacancyContent(_currentUserService.GetUserName()),
            "New Apply");

        return application.Id;
    }
    private async Task<bool> ApplicationExits(string applicationSlug)
    {
        var application = await _context.Applications.FirstOrDefaultAsync(a => a.Slug.Equals(applicationSlug));
        if (application == null)
            return false;
        return true;
    }

    private string GenerateSlug(string username, Vacancy vacancy)
    {
        return _slugService.GenerateSlug($"{username}-{vacancy.Slug}");
    }
}