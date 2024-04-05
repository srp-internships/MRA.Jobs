using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Users;
using MRA.Jobs.Client.Services.Profile;
using MRA.Jobs.Client.Services.Users;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin.Users;

public partial class UsersPage
{
    [Inject] private IHttpClientService Client { get; set; }
    [Inject] private IUserProfileService UserProfileService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IUsersService UsersService { get; set; }

    private string _title = "";
    private string _url = "Administration/Users";
    private bool _isLoaded;
    private string _searchString = "";
    private GetPagedListUsersQuery _query = new();
    private MudTable<UserResponse> _table;
    private UserSkillsResponse _allSkills;
    private string SelectedSkills { get; set; } = "";
    private IEnumerable<string> Options { get; set; } = new HashSet<string>();


    protected override async Task OnInitializedAsync()
    {
        SetTitlePage();
        NavigationManager.LocationChanged += HandleLocationChanged;

        _allSkills = await UserProfileService.GetAllSkills();
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

    private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
    {
        SetTitlePage();
    }

    private void SetTitlePage()
    {
        if (NavigationManager.Uri.Contains("/Administration/Users", StringComparison.OrdinalIgnoreCase))
        {
            _title = ContentService["UserManager:Users"];
            _url = "Administration/Users";
        }

        if (NavigationManager.Uri.Contains("/Administration/Candidates", StringComparison.OrdinalIgnoreCase))
        {
            _title = ContentService["Candidates"];
            _url = "Administration/Candidates";
        }


        StateHasChanged();
    }

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
     
        var result = await UsersService.GetPagedListUsers(_query, _url) ;
        return new TableData<UserResponse>() { TotalItems = result.TotalCount, Items = result.Items };
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return string.Join(", ", selectedValues).Trim();
    }
    
}