using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.Vacancies.Responses;
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.Profile;
using MRA.Jobs.Client.Services.VacanciesServices;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin.Users;

public partial class UserProfile
{
    [Inject] private IUserProfileService UserProfileService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IApplicationService ApplicationService { get; set; }
    [Inject] private IHttpClientService HttpClientService { get; set; }
    [Inject] private IVacancyService VacancyService { get; set; }

    [Parameter] public string Username { get; set; }
    private UserProfileResponse _profile = new();
    private List<UserExperienceResponse> _experiences = new();
    private List<UserEducationResponse> _educations = new();
    private UserSkillsResponse _skills = new();
    private List<VacancyDto> _vacancies = new();
    private VacancyDto _selectedVacancy = new();
    private string _searchStatusName = "";
    private MudDateRangePicker _picker;
    private DateRange _dateRange;

    private int _activeIndex;

    private MudTable<ApplicationListDto> _table;
    private GetApplicationsByFiltersQuery _query = new();
    private bool _isLoaded;

    protected override async Task OnInitializedAsync()
    {
        _vacancies = await VacancyService.GetAllVacancies();
        if (NavigationManager.Uri.Contains("/applications", StringComparison.OrdinalIgnoreCase))
        {
            _activeIndex = 1;
            var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("filters", out var filters))
            {
                var decodedFilters = HttpUtility.UrlDecode(filters).Split(',');
                var filterDictionary = decodedFilters.Select(filter =>
                {
                    var parts = filter.Split(new[] { '@', '=', }, 2);
                    return new { Key = parts[0], Value = parts[1] };
                }).ToDictionary(x => x.Key, x => x.Value.Replace("=", ""));

                _selectedVacancy.Title = filterDictionary.GetValueOrDefault("Vacancy.Title");
                if (filterDictionary.TryGetValue("Status", out var status))
                {
                    _searchStatusName =
                        ((ApplicationStatusDto.ApplicationStatus)int.Parse(status.Replace("=", ""))).ToString();
                }

                if (filterDictionary.TryGetValue("AppliedAt>", out var startDate))
                {
                    if (_dateRange == null) _dateRange = new DateRange();
                    _dateRange.Start = DateTime.Parse(startDate);
                }

                if (filterDictionary.TryGetValue("AppliedAt<", out var endDate))
                {
                    if (_dateRange == null) _dateRange = new DateRange();
                    _dateRange.End = DateTime.Parse(endDate).AddDays(-1);
                }
            }
        }

        try
        {
            _profile = await UserProfileService.Get(Username);
            _experiences = (await UserProfileService.GetExperiencesByUser(Username)).Result;
            _educations = (await UserProfileService.GetEducationsByUser(Username)).Result;
            _skills = await UserProfileService.GetUserSkills(Username);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }

        _isLoaded = true;
        StateHasChanged();
    }

    private async Task<TableData<ApplicationListDto>> GetApplications(TableState state)
    {
        _query.Page = state.Page + 1;
        _query.PageSize = state.PageSize;
        _query.Filters = GetFilters();

        var response =
            await ApplicationService.GetAllApplications(_query);

        UpdateUrl();

        return new TableData<ApplicationListDto>() { TotalItems = response.TotalCount, Items = response.Items };
    }

    private string GetFilters()
    {
        var filters = new List<string>();
        if (!string.IsNullOrEmpty(_selectedVacancy.Title)) filters.Add($"Vacancy.Title@={_selectedVacancy.Title}");
        if (Enum.TryParse(typeof(ApplicationStatusDto.ApplicationStatus), _searchStatusName, out var statusValue))
            filters.Add($"Status=={(int)statusValue}");
        if (_dateRange != null)
            filters.Add($"AppliedAt>={_dateRange.Start},AppliedAt<={_dateRange.End.Value.AddDays(1)}");

        filters.Add($"ApplicantUsername=={Username}");

        return filters.Any() ? HttpUtility.UrlEncode(string.Join(",", filters)) : null;
    }

    private void PersonalDataTab()
    {
        NavigationManager.NavigateTo($"User/{Username}");
    }

    private void ApplicationsTab()
    {
        NavigationManager.NavigateTo($"User/{Username}/Applications");
    }

    private Task<IEnumerable<VacancyDto>> SearchVacancies(string value)
    {
        return Task.FromResult<IEnumerable<VacancyDto>>(_vacancies.Where(v => v.Title.Contains(value)).ToList());
    }

    private void UpdateUrl()
    {
        var properties = new Dictionary<string, string> { { "Filters", _query.Filters } };

        var queryParameters = properties.Where(property => !property.Value.IsNullOrEmpty())
            .ToDictionary(property => property.Key, property => property.Value);

        queryParameters.Add("page", _query.Page.ToString());
        queryParameters.Add("pageSize", _query.PageSize.ToString());

        var url = QueryHelpers.AddQueryString($"User/{Username}/Applications", queryParameters);
        NavigationManager.NavigateTo(url);
    }
}