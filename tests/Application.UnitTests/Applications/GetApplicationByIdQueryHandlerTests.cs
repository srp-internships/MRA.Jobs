using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;

namespace MRA.Jobs.Application.UnitTests.Applications;

public class GetApplicationByIdQueryHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetApplicationByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    private GetApplicationByIdQueryHandler _handler;

    [Test]
    [Ignore("игнориуем изза TimeLine")]
    public async Task Handle_GivenValidQuery_ShouldReturnApplicationDetailsDTO()
    {
        GetByIdApplicationQuery query = new GetByIdApplicationQuery { Id = Guid.NewGuid() };

        Domain.Entities.Application application = new Domain.Entities.Application
        {
            Id = query.Id,
            CoverLetter =
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
            CV = "https://www.lipsum.com/",
            ApplicantId = Guid.NewGuid(),
            VacancyId = Guid.NewGuid(),
            Status = ApplicationStatus.Submitted
        };
        _dbContextMock.Setup(x => x.Applications.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(application);

        // Act
        ApplicationDetailsDto result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(application.Id);
        result.ApplicantId.Should().Be(application.ApplicantId);
        result.VacancyId.Should().Be(application.VacancyId);
        result.CoverLetter.Should().Be(application.CoverLetter);
        result.Cv.Should().Be(application.CV);
        result.StatusId.Should().Be((int)application.Status);
    }
}