using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;
using MRA.Jobs.Client.Services.ApplicationService;
using MRA.Jobs.Client.Services.Profile;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class Candidates
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IUserProfileService UserService { get; set; }
    [Inject] private IApplicationService ApplicationService { get; set; }

    private List<UserResponse> _candidates = new();
    private UserSkillsResponse _allSkills;

    private GetCandidatesQuery _query = new();

    private string SelectedSkills { get; set; } = "";
    private IEnumerable<string> Options { get; set; } = new HashSet<string>();
    private string _searchFullName = "";
    private string _searchPhoneNumber = "";
    private string _searchEmail = "";
    private bool _clearButton;

    protected override async Task OnInitializedAsync()
    {
        _allSkills = await UserService.GetAllSkills();
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
            _searchFullName = fullName;
        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("PhoneNumber", out var phoneNumber))
            _searchPhoneNumber = phoneNumber;
        if (QueryHelpers.ParseQuery(currentUri.Query).TryGetValue("Email", out var email))
            _searchEmail = email;
        StateHasChanged();
        await Search();
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return string.Join(", ", selectedValues).Trim();
    }

    private async Task Search()
    {
        _query.PhoneNumber = _searchPhoneNumber;
        _query.Skills = SelectedSkills;
        _query.FullName = _searchFullName;
        _query.Email = _searchEmail;
        _candidates = await ApplicationService.GetCandidates(_query);
        StateHasChanged();
        UpdateUrl();
    }

    private void UpdateUrl()
    {
        var properties = new Dictionary<string, string>
        {
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

        var url = QueryHelpers.AddQueryString("Candidates", queryParameters);
        NavigationManager.NavigateTo(url);
    }

    private async Task Clear()
    {
        _searchFullName = "";
        _searchEmail = "";
        _searchPhoneNumber = "";
        SelectedSkills = String.Empty;
        await Search();
    }
}