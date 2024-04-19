using System.Runtime.InteropServices;
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
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.Profile;
using MRA.Jobs.Client.Services.VacanciesServices;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Applications;

public partial class Applications
{
    [Inject] private IHttpClientService HttpClientService { get; set; }
    [Inject] private IVacancyService VacancyService { get; set; }
    [Inject] private IUserProfileService UserProfileService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IApplicationService ApplicationService { get; set; }

    private GetApplicationsByFiltersQuery _query = new();
    private bool _isLoad;
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
    private bool _isExpanded;
    private bool _clearButton;
    private IEnumerable<string> Options { get; set; } = new HashSet<string>();
    private string SelectedSkills { get; set; } = "";


    protected override async Task OnParametersSetAsync()
    {
          _vacancies = await VacancyService.GetAllVacancies();
        _allSkills = await UserProfileService.GetAllSkills();
        if (_allSkills != null)
        {
            _allSkills.Skills = _allSkills.Skills.Distinct().OrderBy(x => x).ToList();
        }

        var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("Skills", out var skills))
        {
            SelectedSkills = skills;
            Options = skills.ToList();
        }

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("FullName", out var fullName))
            _selectedFullName = fullName;
        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("PhoneNumber", out var phoneNumber))
            _selectedPhoneNumber = phoneNumber;
        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("Email", out var email))
            _selectedEmail = email;
        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("IsExpanded", out var isExpanded))
            _isExpanded = bool.Parse(isExpanded);

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("ClearButton", out var clearButton))
            _clearButton = bool.Parse(clearButton);

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("filters", out var filters))
        {
            var decodedFilters = HttpUtility.UrlDecode(filters).Split(',');
            var filterDictionary = decodedFilters.Select(filter =>
            {
                var parts = filter.Split(new[] { '@', '=', }, 2);
                return new { Key = parts[0], Value = parts[1] };
            }).ToDictionary(x => x.Key, x => x.Value);

            _selectedVacancy.Title = filterDictionary.GetValueOrDefault("Vacancy.Title").Replace("=", "");
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

        StateHasChanged();
        _isLoad = true;
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

    private void ExpandChanged()
    {
        _isExpanded = !_isExpanded;
        UpdateUrl();
    }

    private async Task<TableData<ApplicationListDto>> GetApplications(TableState state)
    {
        _query = new GetApplicationsByFiltersQuery()
        {
            Page = state.Page + 1,
            PageSize = state.PageSize,
            Skills = SelectedSkills.Trim(),
            FullName = _selectedFullName.Trim(),
            Email = _selectedEmail.Trim(),
            PhoneNumber = _selectedPhoneNumber.Trim(),
            Filters = GetFilters(),
        };

        var response = await ApplicationService.GetAllApplications(_query);

        UpdateUrl();

        return new TableData<ApplicationListDto>() { TotalItems = response.TotalCount, Items = response.Items };
    }

    private void UpdateUrl()
    {
        var properties = new Dictionary<string, string>
        {
            { "Filters", _query.Filters },
            { "Skills", _query.Skills },
            { "FullName", _query.FullName },
            { "PhoneNumber", _query.PhoneNumber },
            { "Email", _query.Email }
        };

        var queryParameters = properties.Where(property => !property.Value.IsNullOrEmpty())
            .ToDictionary(property => property.Key, property => property.Value);

        if (!queryParameters.IsNullOrEmpty())
        {
            _clearButton = true;
            StateHasChanged();
        }

        queryParameters.Add("page", _query.Page.ToString());
        queryParameters.Add("pageSize", _query.PageSize.ToString());

        if (_isExpanded) queryParameters.Add("IsExpanded", _isExpanded.ToString());
        if (_clearButton) queryParameters.Add("ClearButton", _clearButton.ToString());

        var url = QueryHelpers.AddQueryString("Dashboard/Applications", queryParameters);
        NavigationManager.NavigateTo(url);
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

    private async Task Clear()
    {
        Options = new HashSet<string>();
        _selectedEmail = string.Empty;
        _selectedVacancy = new VacancyDto();
        _selectedEmail = string.Empty;
        _searchStatusName = string.Empty;
        _selectedFullName = string.Empty;
        _selectedPhoneNumber = string.Empty;
        SelectedSkills = String.Empty;
        _dateRange = null;
        UpdateUrl();
        _clearButton = false;
        await _table.ReloadServerData();
    }
}