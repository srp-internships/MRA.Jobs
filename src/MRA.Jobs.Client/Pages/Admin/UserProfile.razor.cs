using Microsoft.AspNetCore.Components;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Jobs.Client.Services.Profile;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class UserProfile
{
    [Inject] private IUserProfileService UserProfileService { get; set; }
    [Parameter] public string Username { get; set; }
    private UserProfileResponse _profile = new ();
    private List<UserExperienceResponse> _experiences = new();
    private List<UserEducationResponse> _educations = new();
    private UserSkillsResponse _skills = new();

    protected override async Task OnInitializedAsync()
    {
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
    }
}