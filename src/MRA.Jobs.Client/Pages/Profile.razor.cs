using System.Net;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
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
            Snackbar.Add("Please check your email, we sent verification link", Severity.Info);
        else
            Snackbar.Add("Server not responding, try again latter", Severity.Error);
    }

    private async Task Verify()
    {
        SmsVerificationCodeStatus response =
            await UserProfileService.CheckConfirmationCode(_profile.PhoneNumber, _confirmationCode);
        if (response == SmsVerificationCodeStatus.CodeVerifyFailure)
        {
            Snackbar.Add("Code is incorrect or expired", Severity.Error);
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

        if (!user.Identity!.IsAuthenticated)
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
        Snackbar.Add("Server is not responding, please try later", Severity.Error);
    }

    private async Task BadRequestResponse(HttpResponseMessage response)
    {
        var customProblemDetails = await response.Content.ReadFromJsonAsync<CustomProblemDetails>();
        if (customProblemDetails.Detail != null)
        {
            Snackbar.Add(customProblemDetails.Detail, Severity.Error);
        }
        else
        {
            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);
            foreach (var error in errorResponse.Errors)
            {
                var errorMessage = string.Join(", ", error.Value);
                Snackbar.Add(errorMessage, Severity.Error);
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
                        Guid id = (Guid)idProperty!.GetValue(item)!;

                        switch (itemType.Name)
                        {
                            case "UserEducationResponse":
                                result = await UserProfileService.DeleteEducationAsync(id);
                                break;
                            case "UserExperienceResponse":
                                result = await UserProfileService.DeleteExperienceAsync(id);
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
            Snackbar.Add("Profile updated successfully.", Severity.Success);
            _profile = await UserProfileService.Get();
        }
        else
        {
            Snackbar.Add(result, Severity.Error);
        }

        _processing = false;
        StateHasChanged();
    }

    #region education

    List<UserEducationResponse> _educations = new();
    private bool _addEducation;
    CreateEducationDetailCommand _createEducation = new();
    List<UserEducationResponse> _allEducations = new();


    Guid _editingCardId;
    UpdateEducationDetailCommand _educationUpdate;

    private async Task GetEducations()
    {
        try
        {
            _educations = await UserProfileService.GetEducationsByUser();
            _allEducations = await UserProfileService.GetAllEducations();
        }
        catch (Exception)
        {
            ServerNotResponding();
        }
    }

    private async Task<IEnumerable<string>> SearchEducation(string value)
    {
        await Task.Delay(5);

        return _allEducations.Where(e => e.Speciality
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.Speciality).Distinct()
            .ToList();
    }

    private async Task<IEnumerable<string>> SearchUniversity(string value)
    {
        await Task.Delay(5);

        return _allEducations.Where(e => e.University
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.University).Distinct()
            .ToList();
    }

    private void EditButtonClicked(Guid cardId, UserEducationResponse educationResponse)
    {
        _editingCardId = cardId;
        _educationUpdate = new UpdateEducationDetailCommand()
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
            if (_createEducation.EndDate == null)
                _createEducation.EndDate = default(DateTime);

            var response = await UserProfileService.CreateEducationAsуnc(_createEducation);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Add Education successfully.", Severity.Success);
                _addEducation = false;
                _createEducation = new CreateEducationDetailCommand();
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
        _addEducation = false;
        _createEducation = new CreateEducationDetailCommand();
    }

    private async void CancelButtonClicked_UpdateEducation()
    {
        _editingCardId = Guid.NewGuid();
        await GetEducations();
        StateHasChanged();
    }

    private async Task UpdateEducationHandle()
    {
        _processing = true;
        try
        {
            var result = await UserProfileService.UpdateEducationAsync(_educationUpdate);
            if (result.IsSuccessStatusCode)
            {
                _editingCardId = Guid.NewGuid();
                Snackbar.Add("Update Education successfully.", Severity.Success);

                await GetEducations();
                StateHasChanged();
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
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

    List<UserExperienceResponse> _experiences = new();
    private bool _addExperience;
    CreateExperienceDetailCommand _createExperience = new();
    UpdateExperienceDetailCommand _experienceUpdate;
    List<UserExperienceResponse> _allExperiences = new();
    private Guid _editingCardExperienceId;

    private async Task GetExperiences()
    {
        _experiences = await UserProfileService.GetExperiencesByUser();
        _allExperiences = await UserProfileService.GetAllExperiences();
    }

    private async Task<IEnumerable<string>> SearchJobTitle(string value)
    {
        await Task.Delay(5);

        return _allExperiences.Where(e => e.JobTitle
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.JobTitle).Distinct()
            .ToList();
    }

    private async Task<IEnumerable<string>> SearchCompanyName(string value)
    {
        await Task.Delay(5);

        return _allExperiences.Where(e => e.CompanyName
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.CompanyName).Distinct()
            .ToList();
    }

    private async Task<IEnumerable<string>> SearchAddress(string value)
    {
        await Task.Delay(5);

        return _allExperiences.Where(e => e.Address
                .Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(e => e.Address).Distinct()
            .ToList();
    }

    private async Task CreateExperienceHandle()
    {
        _processing = true;
        try
        {
            if (_createExperience.EndDate == null)
                _createExperience.EndDate = default(DateTime);

            var response = await UserProfileService.CreateExperienceAsync(_createExperience);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Add Experience successfully.", Severity.Success);

                _addExperience = false;
                _createExperience = new CreateExperienceDetailCommand();
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
        _addExperience = false;
        _createExperience = new CreateExperienceDetailCommand();
        await GetExperiences();
    }

    private async void CancelButtonClicked_UpdateExperience()
    {
        _editingCardExperienceId = Guid.NewGuid();
        await GetExperiences();
        StateHasChanged();
    }

    private void EditCardExperienceButtonClicked(Guid cardExperienceId, UserExperienceResponse experienceResponse)
    {
        _editingCardExperienceId = cardExperienceId;
        _experienceUpdate = new UpdateExperienceDetailCommand()
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
            var result = await UserProfileService.UpdateExperienceAsync(_experienceUpdate);
            if (result.IsSuccessStatusCode)
            {
                Snackbar.Add("Update Education successfully.", Severity.Success);

                _editingCardExperienceId = Guid.NewGuid();
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

    private UserSkillsResponse _userSkills;
    private UserSkillsResponse _allSkills;
    string _newSkills = "";

    private async Task GetSkills()
    {
        try
        {
            _userSkills = await UserProfileService.GetUserSkills();
            _allSkills = await UserProfileService.GetAllSkills();
        }
        catch (HttpRequestException)
        {
            ServerNotResponding();
        }
    }

    async void Closed(MudChip chip)
    {
        await ConfirmDelete(_userSkills.Skills, chip.Text);
    }

    private async Task OnBlur()
    {
        await AddSkills();
    }

    private async Task AddSkills(string foundSkill = null)
    {
        if (foundSkill != null)
            _newSkills = foundSkill;

        try
        {
            if (!string.IsNullOrWhiteSpace(_newSkills))
            {
                var userSkillsCommand = new AddSkillsCommand();
                var skills = _newSkills.Split(',', StringSplitOptions.RemoveEmptyEntries);
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
                    Snackbar.Add("Add Skills successfully.", Severity.Success);

                    _newSkills = "";
                    _userSkills = result;
                    StateHasChanged();
                }
                
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
        var userSkillsSet = new HashSet<string>(_userSkills.Skills);
        return _allSkills.Skills
            .Where(s => s.Contains(value, StringComparison.InvariantCultureIgnoreCase) && !userSkillsSet.Contains(s))
            .ToList();
    }

    #endregion
}