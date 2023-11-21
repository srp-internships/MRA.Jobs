using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Responses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MRA.Identity.Application.Services.GeneratePdfCV;
public class InvoiceDocument : IDocument
{
    private readonly UserProfileResponse _userProfile;
    private readonly UserSkillsResponse _userSkillsResponse;
    private readonly List<UserEducationResponse> _userEducations;
    private readonly List<UserExperienceResponse> _userExperience;
    TextStyle titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

    public InvoiceDocument(UserProfileResponse userProfile, UserSkillsResponse userSkillsResponse,
        List<UserEducationResponse> userEducations, List<UserExperienceResponse> userExperience)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        _userProfile = userProfile;
        _userSkillsResponse = userSkillsResponse;
        _userEducations = userEducations;
        _userExperience = userExperience;
    }
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);


                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {


        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"{_userProfile.FirstName} {_userProfile.LastName}").Style(titleStyle).FontSize(14);

                column.Item().Text(text =>
                {
                    text.Span("Email: ").SemiBold();
                    text.Span($"{_userProfile.Email}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Phone Number: ").SemiBold();
                    text.Span($"{_userProfile.PhoneNumber}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Date Of Birth: ").SemiBold();
                    text.Span($"{_userProfile.DateOfBirth.ToString("dd.MM.yyyy")}");
                });

                if (_userProfile.AboutMyself != null)
                {
                    column.Item().Text(text =>
                    {
                        text.Span("About: ").SemiBold();
                        text.Span($"{_userProfile.AboutMyself}");
                    });
                }
            });
        });
    }

    void ComposeContent(IContainer container)
    {

        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Element(ComposeEducations);

            column.Item().Element(ComposeExperience);

            if (_userSkillsResponse.Skills.Count > 0)
                column.Item().PaddingTop(25).Element(ComposeSkills);
        });
    }


    void ComposeEducations(IContainer container)
    {
        if (_userEducations.Any())
        {
            container.Column(column =>
            {
                column.Item().BorderBottom(1).PaddingBottom(5).Text("Educations").Style(titleStyle).FontSize(14);
                for (int i = 0; i < _userEducations.Count / 3; i += 3)
                {
                    List<UserEducationResponse> sublist = _userEducations.GetRange(i, i + 3);

                    column.Item().Row(row =>
                    {
                        foreach (var education in sublist)
                            row.RelativeItem().Component(new EducationComponent(education));
                    });
                    column.Spacing(5);
                }
                List<UserEducationResponse> sublistLastItems = _userEducations.Skip(_userEducations.Count - (_userEducations.Count % 3)).ToList();

                column.Item().Row(row =>
                {
                    foreach (var education in sublistLastItems)
                        row.RelativeItem().Component(new EducationComponent(education));
                });
            });
        }
    }

    void ComposeExperience(IContainer container)
    {
        if (_userExperience.Any())
        {
            container.Column(column =>
            {

                column.Item().BorderBottom(1).PaddingBottom(5).Text("Experiences").Style(titleStyle).FontSize(14);

                for (int i = 0; i < _userExperience.Count / 3; i += 3)
                {
                    List<UserExperienceResponse> sublist = _userExperience.GetRange(i, i + 3);


                    column.Item().Row(row =>
                    {
                        foreach (var experience in sublist)
                            row.RelativeItem().Component(new ExperienceComponent(experience));
                    });
                    column.Spacing(5);
                }
                List<UserExperienceResponse> sublistLastItems = _userExperience.Skip(_userExperience.Count - (_userExperience.Count % 3)).ToList();

                column.Item().Row(row =>
                {
                    foreach (var experience in sublistLastItems)
                        row.RelativeItem().Component(new ExperienceComponent(experience));
                });
            });
        }
    }

    void ComposeSkills(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(5);
            column.Item().BorderBottom(1).PaddingBottom(5).Text("Skills").SemiBold().Style(titleStyle).FontSize(14);
            var skillsString = "";
            foreach (var item in _userSkillsResponse.Skills)
            {
                skillsString += $"{item}, ";
            }
            column.Item().Text(skillsString.Remove(skillsString.Length - 3, 2));
        });
    }
}

public class EducationComponent : IComponent
{
    private readonly UserEducationResponse _userEducationResponse;

    public EducationComponent(UserEducationResponse userEducationResponse)
    {
        _userEducationResponse = userEducationResponse;
    }
    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);
            column.Item().Text(text =>
            {
                text.Span("Specialty: ").SemiBold();
                text.Span(_userEducationResponse.Speciality);
            });

            column.Item().Text($"Universiry: {_userEducationResponse.University}");
            column.Item().Text(_userEducationResponse.StartDate.HasValue ?
                                $"Start Date: {_userEducationResponse.StartDate.Value.ToString("dd.MM.yyyy")}" :
                                "Start Date: Not available");

            if (_userEducationResponse.UntilNow == false)
            {
                column.Item().Text(_userEducationResponse.EndDate.HasValue ?
                                   $"End Date: {_userEducationResponse.EndDate.Value.ToString("dd.MM.yyyy")}" :
                                   "End Date: Not available");
            }

            else
            {
                column.Item().Text("Until Now");
            }
        });
    }
}

public class ExperienceComponent : IComponent
{
    private readonly UserExperienceResponse _userExperienceResponse;

    public ExperienceComponent(UserExperienceResponse userExperienceResponse)
    {
        _userExperienceResponse = userExperienceResponse;
    }
    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);
            column.Item().Text(text =>
            {
                text.Span("Job Title: ").SemiBold();
                text.Span(_userExperienceResponse.JobTitle);
            });

            column.Item().Text($"Company Name: {_userExperienceResponse.CompanyName}");
            column.Item().Text(_userExperienceResponse.StartDate.HasValue ?
                                $"Start Date: {_userExperienceResponse.StartDate.Value.ToString("dd.MM.yyyy")}" :
                                "Start Date: Not available");

            if (_userExperienceResponse.IsCurrentJob == false)
            {
                column.Item().Text(_userExperienceResponse.EndDate.HasValue ?
                                   $"End Date: {_userExperienceResponse.EndDate.Value.ToString("dd.MM.yyyy")}" :
                                   "End Date: Not available");
            }

            else
            {
                column.Item().Text("Current Job");
            }
            column.Item().Text($"Description: {_userExperienceResponse.Description}");
        });
    }
}