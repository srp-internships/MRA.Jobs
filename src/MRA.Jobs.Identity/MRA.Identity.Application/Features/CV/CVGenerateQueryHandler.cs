using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.CV;
using MRA.Identity.Application.Contract.Educations.Query;
using MRA.Identity.Application.Contract.Educations.Responses;
using MRA.Identity.Application.Contract.Experiences.Query;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Identity.Application.Contract.Skills.Queries;
using MRA.Identity.Application.Contract.Skills.Responses;
using MRA.Identity.Application.Features.UserProfiles.Query;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Unit = QuestPDF.Infrastructure.Unit;

namespace MRA.Identity.Application.Features.CV;
public class CVGenerateQueryHandler : IRequestHandler<CVGenerateQuery, MemoryStream>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserHttpContextAccessor _userHttpContext;
    private readonly IMediator _mediator;

    public CVGenerateQueryHandler(IApplicationDbContext dbContext,
        IUserHttpContextAccessor userHttpContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _userHttpContext = userHttpContext;
        _mediator = mediator;
    }

    public async Task<MemoryStream> Handle(CVGenerateQuery request, CancellationToken cancellationToken)
    {
        QuestPDF.Settings.License = LicenseType.Community;


        var userProfile = await _mediator.Send(new GetPofileQuery());
        var userSkills = await _mediator.Send(new GetUserSkillsQuery());
        var userEducations = await _mediator.Send(new GetEducationsByUserQuery());
        var userExperience= await _mediator.Send(new GetExperiencesByUserQuery());

        InvoiceDocument document = new InvoiceDocument(userProfile, userSkills,
            userEducations, userExperience);

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
        return stream;

    }

    public class InvoiceDocument : IDocument
    {
        private readonly UserProfileResponse _userProfile;
        private readonly UserSkillsResponse _userSkillsResponse;
        private readonly List<UserEducationResponse> _userEducations;
        private readonly List<UserExperienceResponse> _userExperionse;

        public InvoiceDocument(UserProfileResponse userProfile, UserSkillsResponse userSkillsResponse,
            List<UserEducationResponse> userEducations, List<UserExperienceResponse> userExperience)
        {
            _userProfile = userProfile;
            _userSkillsResponse = userSkillsResponse;
            _userEducations = userEducations;
            _userExperionse = userExperience;
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
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{_userProfile.FirstName} {_userProfile.LastName}").Style(titleStyle);

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

                row.ConstantItem(100).Height(50).Placeholder();
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Element(ComposeTable);

                if (_userSkillsResponse.Skills.Count > 0)
                    column.Item().PaddingTop(25).Element(ComposeSkills);
            });
        }

        void ComposeTable(IContainer container)
        {
            container
                .Height(250)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter()
                .AlignMiddle()
                .Text("Table").FontSize(16);
        }

        void ComposeEducations(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(5);

                column.Item().Text("Educations");
            });
        }


        void ComposeSkills(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Skills").FontSize(14);
                var skillsString = "";
                foreach (var item in _userSkillsResponse.Skills)
                {
                    skillsString += $"{item}, ";
                }
                column.Item().Text(skillsString.Remove(skillsString.Length - 3, 2));
            });
        }

    }
}