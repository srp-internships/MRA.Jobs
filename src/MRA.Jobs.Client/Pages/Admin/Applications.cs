using System.Web;
using Microsoft.AspNetCore.Components;
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

    private MudDateRangePicker _picker;
    private DateRange _dateRange;
    private MudTable<ApplicationListDto> _table;
    private List<VacancyDto> _vacancies = new();
    private VacancyDto _selectedVacancy = new();
    private UserSkillsResponse _allSkills;
    private string _selectedFullName="";
    private string _selectedEmail ="";
    private string _selectedPhoneNumber="";
    private string _searchStatusName="";
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
            FullName =_selectedFullName.Trim(),
            Email = _selectedEmail.Trim(),
            PhoneNumber = _selectedPhoneNumber.Trim(),
            Filters = GetFilters()
        };

        var response =
            await HttpClientService.GetFromJsonAsync<PagedList<ApplicationListDto>>(
                Configuration.GetJobsUrl("applications"), query);
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

        return filters.Any() ? HttpUtility.UrlEncode(string.Join(",", filters)) : null;
    }
}