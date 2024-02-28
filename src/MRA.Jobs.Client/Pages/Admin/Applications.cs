using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.Vacancies.Responses;
using MRA.Jobs.Client.Services.Profile;
using MRA.Jobs.Client.Services.VacanciesServices;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class Applications
{
    [Inject] private IHttpClientService HttpClientService { get; set; }
    [Inject] private IVacancyService VacancyService { get; set; }
    [Inject] private IUserProfileService UserProfileService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    private MudDateRangePicker _picker;
    private DateRange _dateRange;
    private MudTable<ApplicationListDto> _table;
    private List<VacancyDto> _vacancies = new();
    private VacancyDto _selectedVacancy = new();
    private UserSkillsResponse _allSkills;
    private string _selectedFullName = "";
    private string _selectedEmail = "";
    private string _selectedPhoneNumber = "";
    private string _searchStatusName = "";
    private IEnumerable<string> Options { get; set; } = new HashSet<string>();
    private string SelectedSkills { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        _vacancies = await VacancyService.GetAllVacancies();
        _allSkills = await UserProfileService.GetAllSkills();
        if (_allSkills != null)
        {
            _allSkills.Skills = _allSkills.Skills.Distinct().OrderBy(x => x).ToList();
        }

        var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);


        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("page", out var page))
            _table.CurrentPage = int.Parse(page) - 1;


        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("pageSize", out var pageSize))
            _table.TotalItems = int.Parse(pageSize);


        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("Skills", out var skills))
            SelectedSkills = skills;


        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("filters", out var filters))
        {
            var decodedFilters = HttpUtility.UrlDecode(filters).Split(',');
            var filterDictionary = decodedFilters.Select(filter =>
            {
                var parts = filter.Split(new[] { '@', '=', }, 2);
                return new { Key = parts[0], Value = parts[1] };
            }).ToDictionary(x => x.Key, x => x.Value);

            _selectedVacancy.Title = filterDictionary.GetValueOrDefault("Vacancy.Title");
            if (filterDictionary.TryGetValue("Status", out var status))
            {
                _searchStatusName = ((ApplicationStatusDto.ApplicationStatus)int.Parse(status.Replace("=",""))).ToString();
            }

            if (filterDictionary.TryGetValue("AppliedAt>", out var startDate))
            {
                _dateRange.Start = DateTime.Parse(startDate);
            }

            if (filterDictionary.TryGetValue("AppliedAt<", out var endDate))
            {
                _dateRange.End = DateTime.Parse(endDate).AddDays(-1);
            }
        }
        StateHasChanged();
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return string.Join(", ", selectedValues).Trim();
    }

    private Task<IEnumerable<VacancyDto>> SearchVacancies(string value)
    {
        return Task.FromResult<IEnumerable<VacancyDto>>(_vacancies.Where(v => v.Title.Contains(value)).ToList());
    }

    private async Task<TableData<ApplicationListDto>> GetApplications(TableState state)
    {
        var query = new GetApplicationsByFiltersQuery()
        {
            Page = state.Page + 1,
            PageSize = state.PageSize,
            Skills = SelectedSkills.Trim(),
            FullName = _selectedFullName.Trim(),
            Email = _selectedEmail.Trim(),
            PhoneNumber = _selectedPhoneNumber.Trim(),
            Filters = GetFilters(),
        };

        var response =
            await HttpClientService.GetFromJsonAsync<PagedList<ApplicationListDto>>(
                Configuration.GetJobsUrl("applications"), query);

        var queryParameters = new Dictionary<string, string>
        {
            { "page", query.Page.ToString() }, { "pageSize", query.PageSize.ToString() }
        };
        if (!query.Filters.IsNullOrEmpty()) queryParameters.Add("Filters", query.Filters);
        if (!query.Skills.IsNullOrEmpty()) queryParameters.Add("Skills", query.Skills);
        if (!query.Skills.IsNullOrEmpty()) queryParameters.Add("FullName", query.FullName);
        if (!query.Skills.IsNullOrEmpty()) queryParameters.Add("PhoneNumber", query.PhoneNumber);
        if (!query.Skills.IsNullOrEmpty()) queryParameters.Add("Email", query.Email);

        var url = QueryHelpers.AddQueryString("Admin/Applications", queryParameters);
        NavigationManager.NavigateTo(url);

        if (response.Success && response.Result != null)
        {
            return new TableData<ApplicationListDto>()
            {
                TotalItems = response.Result.TotalCount, Items = response.Result.Items
            };
        }

        return new TableData<ApplicationListDto>() { TotalItems = _table.TotalItems, Items = _table.Items };
    }

    private string GetFilters()
    {
        var filters = new List<string>();
        if (!string.IsNullOrEmpty(_selectedVacancy.Title)) filters.Add($"Vacancy.Title@={_selectedVacancy.Title}");
        if (Enum.TryParse(typeof(ApplicationStatusDto.ApplicationStatus), _searchStatusName, out var statusValue))
            filters.Add($"Status=={(int)statusValue}");
        if (_dateRange != null)
            filters.Add($"AppliedAt>={_dateRange.Start},AppliedAt<={_dateRange.End.Value.AddDays(1)}");

        return filters.Any() ? HttpUtility.UrlEncode(string.Join(",", filters)) : null;
    }
}