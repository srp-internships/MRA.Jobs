using System.Net;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Commands.UpdateProfile;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Command;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Jobs.Client.Components.Dialogs;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Services.Auth;
using MudBlazor;
using Newtonsoft.Json;

namespace MRA.Jobs.Client.Pages;

public partial class Profile
{
    [Inject] private IAuthService AuthService { get; set; }

    private bool _processing;
    private bool _tryButton;
    private bool _codeSent;
    private int? _confirmationCode;
    private bool _isPhoneNumberNull = true;

    private int _active;

    private void ActiveNavLink(int active)
    {
        if (_profile != null)
        {
            _active = active;
            return;
        }

        ServerNotResponding();
    }

    private UserProfileResponse _profile;
    private readonly UpdateProfileCommand _updateProfileCommand = new UpdateProfileCommand();

    private async Task SendCode()
    {
        bool response = await UserProfileService.SendConfirmationCode(_profile.PhoneNumber);
        if (response) _codeSent = true;
    }

    private async Task SendEmailConfirms()
    {
        var response = await AuthService.ResendVerificationEmail();
        if (response.IsSuccessStatusCode)
            Snackbar.Add("Please check your email, we are send verification", Severity.Info);
        else
            Snackbar.Add("Server not responding, try again latter", Severity.Error);
    }

    private async Task Verify()
    {
        SmsVerificationCodeStatus response =
            await UserProfileService.CheckConfirmationCode(_profile.PhoneNumber, _confirmationCode);
        if (response == SmsVerificationCodeStatus.CodeVerifyFailure)
        {
            Snackbar.Add("Code is incorrect or expired", MudBlazor.Severity.Error);
        }
        else
        {
            _profile = await UserProfileService.Get();
            _codeSent = false;
        }
    }

    protected override async void OnInitialized()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity.IsAuthenticated)
        {
            Navigation.NavigateTo("sign-in");
            return;
        }

        await Load();
    }

    private async Task Load()
    {
        _tryButton = false;
        StateHasChanged();


        try
        {
            _profile = await UserProfileService.Get();

            _updateProfileCommand.AboutMyself = _profile.AboutMyself;
            _updateProfileCommand.DateOfBirth = _profile.DateOfBirth;
            _updateProfileCommand.Email = _profile.Email;
            _updateProfileCommand.FirstName = _profile.FirstName;
            _updateProfileCommand.LastName = _profile.LastName;
            _updateProfileCommand.Gender = _profile.Gender;
            _updateProfileCommand.PhoneNumber = _profile.PhoneNumber;
            _isPhoneNumberNull = !_updateProfileCommand.PhoneNumber.IsNullOrEmpty();
        }
        catch (Exception)
        {
            ServerNotResponding();
            _tryButton = true;
            StateHasChanged();
        }


        await GetEducations();

        await GetExperiences();

        await GetSkills();

        StateHasChanged();
    }

    private void ServerNotResponding()
    {
        Snackbar.Add("Server is not responding, please try later", MudBlazor.Severity.Error);
    }

    private async Task BadRequestResponse(HttpResponseMessage response)
    {
        var customProblemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        if (customProblemDetails.Detail != null)
        {
            Snackbar.Add(customProblemDetails.Detail, MudBlazor.Severity.Error);
        }
        else
        {
            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);
            foreach (var error in errorResponse.Errors)
            {
                var errorMessage = string.Join(", ", error.Value);
                Snackbar.Add(errorMessage, MudBlazor.Severity.Error);
            }
        }
    }

    private async Task ConfirmDelete<T>(IList<T> collection, T item)
    {
        var parameters = new DialogParameters<DialogMudBlazor>();
        parameters.Add(x => x.ContentText,
            "Do you really want to delete these records? This process cannot be undone.");
        parameters.Add(x => x.ButtonText, "Delete");
        parameters.Add(x => x.Color, Color.Error);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = DialogService.Show<DialogMudBlazor>("Delete", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            Delete(collection, item);
        }
    }

    private async void Delete<T>(ICollection<T> collection, T item)
    {
        if (collection.Contains(item))
        {
            Type itemType = item.GetType();
            var idProperty = itemType.GetProperty("Id");
            var result = new HttpResponseMessage();
            if (idProperty != null || itemType.Name == "String")
            {
                try
                {
                    if (itemType.Name == "String")
                    {
                        result = await UserProfileService.RemoveSkillAsync(item.ToString());
                    }
                    else
                    {
                        Guid id = (Guid)idProperty.GetValue(item);

                        switch (itemType.Name)
                        {
                            case "UserEducationResponse":
                                result = await UserProfileService.DeleteEducationAsync(id);
                                break;
                            case "UserExperienceResponse":
                                result = await UserProfileService.DeleteExperienceAsync(id);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    ServerNotResponding();
                    return;
                }

                if (result.IsSuccessStatusCode)
                {
                    collection.Remove(item);
                    StateHasChanged();
                }
            }
        }
    }

    private async void UpdateProfile()
    {
        _processing = true;
        var result = await UserProfileService.Update(_updateProfileCommand);
        if (result == "")
        {
            Snackbar.Add("Profile updated successfully.", MudBlazor.Severity.Success);
            _profile = await UserProfileService.Get();
        }
        else
        {
            Snackbar.Add(result, MudBlazor.Severity.Error);
        }

        _processing = false;
        StateHasChanged();
    }

    #region education

    List<UserEducationResponse> educations = new List<UserEducationResponse>();
    private bool addEducation = false;
    CreateEducationDetailCommand createEducation = new CreateEducationDetailCommand();
    List<UserEducationResponse> allEducctions = new List<UserEducationResponse>();


    Guid editingCardId = new Guid();
    UpdateEducationDetailCommand educationUpdate = null;

    private async Task GetEducations()
    {
        try
        {
            educations = await UserProfileService.GetEducationsByUser();
            allEducctions = await UserProfileService.GetAllEducations();
        }
        catch (Exception)
        {
            ServerNotResponding();
        }
    }

    private async Task<IEnumerable<string>> SearchEducation(string value)
    {
        await Task.Delay(5);

        return allEducctions.Where(e => e.Speciality
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.Speciality).Distinct()
            .ToList();
    }

    private async Task<IEnumerable<string>> SearchUniversity(string value)
    {
        await Task.Delay(5);

        return allEducctions.Where(e => e.University
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.University).Distinct()
            .ToList();
    }

    private void EditButtonClicked(Guid cardId, UserEducationResponse educationResponse)
    {
        editingCardId = cardId;
        educationUpdate = new UpdateEducationDetailCommand()
        {
            EndDate = educationResponse.EndDate.HasValue ? educationResponse.EndDate.Value : default(DateTime),
            StartDate =
                educationResponse.StartDate.HasValue ? educationResponse.StartDate.Value : default(DateTime),
            University = educationResponse.University,
            Speciality = educationResponse.Speciality,
            Id = educationResponse.Id,
            UntilNow = educationResponse.UntilNow
        };
    }

    private async Task CreateEducationHandle()
    {
        _processing = true;
        try
        {
            if (createEducation.EndDate == null)
                createEducation.EndDate = default(DateTime);

            var response = await UserProfileService.CreateEducationAsуnc(createEducation);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Add Education successfully.", MudBlazor.Severity.Success);
                addEducation = false;
                createEducation = new CreateEducationDetailCommand();
                await GetEducations();
                StateHasChanged();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await BadRequestResponse(response);
            }
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }

        _processing = false;
    }

    private void CancelButtonClicked_CreateEducation()
    {
        addEducation = false;
        createEducation = new CreateEducationDetailCommand();
    }

    private async void CancelButtonClicked_UpdateEducation()
    {
        editingCardId = Guid.NewGuid();
        await GetEducations();
        StateHasChanged();
    }

    private async Task UpdateEducationHandle()
    {
        _processing = true;
        try
        {
            var result = await UserProfileService.UpdateEducationAsync(educationUpdate);
            if (result.IsSuccessStatusCode)
            {
                editingCardId = Guid.NewGuid();
                Snackbar.Add("Update Education successfully.", MudBlazor.Severity.Success);

                await GetEducations();
                StateHasChanged();
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                await BadRequestResponse(result);
            }
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }

        _processing = false;
    }

    #endregion

    #region Experience

    List<UserExperienceResponse> experiences = new List<UserExperienceResponse>();
    private bool addExperience = false;
    CreateExperienceDetailCommand createExperience = new CreateExperienceDetailCommand();
    UpdateExperienceDetailCommand experienceUpdate = null;
    List<UserExperienceResponse> allExperiences = new List<UserExperienceResponse>();
    private Guid editingCardExperienceId;

    private async Task GetExperiences()
    {
        experiences = await UserProfileService.GetExperiencesByUser();
        allExperiences = await UserProfileService.GetAllExperiences();
    }

    private async Task<IEnumerable<string>> SearchJobTitle(string value)
    {
        await Task.Delay(5);

        return allExperiences.Where(e => e.JobTitle
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.JobTitle).Distinct()
            .ToList();
    }

    private async Task<IEnumerable<string>> SearchCompanyName(string value)
    {
        await Task.Delay(5);

        return allExperiences.Where(e => e.CompanyName
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.CompanyName).Distinct()
            .ToList();
    }

    private async Task<IEnumerable<string>> SearchAddress(string value)
    {
        await Task.Delay(5);

        return allExperiences.Where(e => e.Address
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.Address).Distinct()
            .ToList();
    }

    private async Task CreateExperienceHandle()
    {
        _processing = true;
        try
        {
            if (createExperience.EndDate == null)
                createExperience.EndDate = default(DateTime);

            var response = await UserProfileService.CreateExperienceAsync(createExperience);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Add Experience successfully.", MudBlazor.Severity.Success);

                addExperience = false;
                createExperience = new CreateExperienceDetailCommand();
                await GetExperiences();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
                await BadRequestResponse(response);
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }

        _processing = false;
    }

    private async void CancelButtonClicked_CreateExperience()
    {
        addExperience = false;
        createExperience = new CreateExperienceDetailCommand();
        await GetExperiences();
    }

    private async void CancelButtonClicked_UpdateExperience()
    {
        editingCardExperienceId = Guid.NewGuid();
        await GetExperiences();
        StateHasChanged();
    }

    private void EditCardExperienceButtonClicked(Guid cardExperienceId, UserExperienceResponse experienceResponse)
    {
        editingCardExperienceId = cardExperienceId;
        experienceUpdate = new UpdateExperienceDetailCommand()
        {
            Id = experienceResponse.Id,
            JobTitle = experienceResponse.JobTitle,
            CompanyName = experienceResponse.CompanyName,
            Address = experienceResponse.Address,
            IsCurrentJob = experienceResponse.IsCurrentJob,
            StartDate = experienceResponse.StartDate,
            EndDate = experienceResponse.EndDate
        };
    }

    private async void UpdateExperienceHandle()
    {
        _processing = true;
        try
        {
            var result = await UserProfileService.UpdateExperienceAsync(experienceUpdate);
            if (result.IsSuccessStatusCode)
            {
                Snackbar.Add("Update Education successfully.", MudBlazor.Severity.Success);

                editingCardExperienceId = Guid.NewGuid();
                StateHasChanged();
            }

            if (result.StatusCode == HttpStatusCode.BadRequest)
                await BadRequestResponse(result);
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }

        _processing = false;
    }

    #endregion

    #region Skills

    private UserSkillsResponse UserSkills;
    private UserSkillsResponse allSkills;
    bool isEditing = false;
    string newSkills = "";
    bool isAdding = false;
    List<string> FoundSkills = new List<string>();

    private async Task GetSkills()
    {
        try
        {
            UserSkills = await UserProfileService.GetUserSkills();
            allSkills = await UserProfileService.GetAllSkills();
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }
    }

    async void Closed(MudChip chip)
    {
        await ConfirmDelete(UserSkills.Skills, chip.Text);
    }

    private async Task KeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Task.Delay(1);
            if (!isAdding)
            {
                isAdding = true;
                await AddSkills();
            }
        }
    }

    private async Task OnBlur()
    {
        await AddSkills();
    }

    private async Task AddSkills(string foundSkill = null)
    {
        if (foundSkill != null)
            newSkills = foundSkill;

        try
        {
            if (!string.IsNullOrWhiteSpace(newSkills))
            {
                var userSkillsCommand = new AddSkillsCommand();
                var skills = newSkills.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var skill in skills)
                {
                    var trimmedSkill = skill.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedSkill))
                    {
                        userSkillsCommand.Skills.Add(trimmedSkill);
                    }
                }

                var result = await UserProfileService.AddSkills(userSkillsCommand);
                if (result != null)
                {
                    Snackbar.Add("Add Skills successfully.", MudBlazor.Severity.Success);

                    newSkills = "";
                    isEditing = false;
                    UserSkills = result;
                    StateHasChanged();
                }

                isAdding = false;
            }
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }
    }


    private async Task<IEnumerable<string>> SearchSkills(string value)
    {
        await Task.Delay(5);
        var userSkillsSet = new HashSet<string>(UserSkills.Skills);
        return allSkills.Skills
            .Where(s => s.Contains(value, StringComparison.InvariantCultureIgnoreCase) && !userSkillsSet.Contains(s))
            .ToList();
    }

    #endregion
}