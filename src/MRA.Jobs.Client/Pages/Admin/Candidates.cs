using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Client.Services.Profile;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class Candidates
{
    [Inject] private IHttpClientService Client { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IUserProfileService UserService { get; set; }

    private bool _isLoaded = false;
    private GetAllUsersByFilters _query = new();
    private MudTable<UserResponse> _table;
    private UserSkillsResponse _allSkills;
    private string SelectedSkills { get; set; } = "";
    private IEnumerable<string> Options { get; set; } = new HashSet<string>();
    private string _searchString = "";

    protected override async Task OnInitializedAsync()
    {
        _allSkills = await UserService.GetAllSkills();
        if (_allSkills != null)
        {
            _allSkills.Skills = _allSkills.Skills.Distinct().OrderBy(x => x).ToList();
        }

        var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

        if (currentUri.Query.IsNullOrEmpty())
        {
            _query.Page = 1;
            _query.PageSize = 10;
        }

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("page", out var page))
            _query.Page = int.Parse(page);

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("pageSize", out var pageSize))
            _query.PageSize = int.Parse(pageSize);

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("Skills", out var skills))
        {
            Options = skills.ToList();
        }

        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("filters", out var filters))
        {
            var filterParts = filters.ToString().Split("@=");
            if (filterParts.Length > 1)
            {
                _searchString = filterParts[1].Replace("|", " ");
            }
        }

        StateHasChanged();
        _isLoaded = true;
        StateHasChanged();
    }
    
    // private string GetMultiSelectionText(List<string> selectedValues)
    // {
    //     return string.Join(", ", selectedValues).Trim();
    // }
    
    private async Task<TableData<UserResponse>> ServerReload(TableState state)
    {
        _query.Page = state.Page + 1;
        _query.PageSize = state.PageSize;
        if (!_searchString.IsNullOrEmpty())
        {
            var searchTerms = _searchString.Replace(",", "|").Split(" ").Select(s => s.Trim());
            _query.Filters = $"(UserName|FirstName|LastName)@={string.Join("|", searchTerms)}";
        }
        else
        {
            _query.Filters = "";
        }

        if (Options != null)
        {
            _query.Skills = string.Join(",", Options.Select(x => x.Trim())).ToString();
        }

        UpdateUri();

        var result = await UserService.GetCandidatesAsync(_query);
        return new TableData<UserResponse>() { TotalItems = result.TotalCount, Items = result.Items };
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return string.Join(", ", selectedValues).Trim();
    }

    private void UpdateUri()
    {
        var queryParam = HttpUtility.ParseQueryString(string.Empty);
        if (!_query.Skills.IsNullOrEmpty()) queryParam["Skills"] = _query.Skills;
        queryParam["Page"] = _query.Page.ToString();
        queryParam["PageSize"] = _query.PageSize.ToString();
        if (!_query.Filters.IsNullOrEmpty()) queryParam["Filters"] = _query.Filters;

        var newUri = $"{NavigationManager.BaseUri}Candidates?{queryParam}";
        NavigationManager.NavigateTo(newUri, forceLoad: false);
    }
}