using MediatR;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.CV;
using MRA.Identity.Application.Contract.Profile.Queries;
using MRA.Identity.Application.Contract.Profile.Responses;
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

        InvoiceDocument document = new InvoiceDocument(userProfile);

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
        return stream;



    }

    public class InvoiceDocument : IDocument
    {
        private readonly UserProfileResponse _userProfile;

        public InvoiceDocument(UserProfileResponse userProfile)
        {
            _userProfile = userProfile;
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
            container
                .PaddingVertical(40)
                .Height(250)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter()
                .AlignMiddle()
                .Text("Content").FontSize(16);
        }

       
    }
}