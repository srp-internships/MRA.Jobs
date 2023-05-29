using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancyTest;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;
public class CreateJobVacancyTestCommandHandlerTests : BaseTestFixture
{
    private CreateJobVacancyTestCommandHandler _handler;
    private Mock<IJobVacancyHttpClientService> _httpClientMock;
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _httpClientMock = new Mock<IJobVacancyHttpClientService>();
        _handler = new CreateJobVacancyTestCommandHandler(
            _dbContextMock.Object,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object,
            _httpClientMock.Object);
    }

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateJobVacancyTest()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var testInfo = new TestInfoDTO
        {
            MaxScore = 10,
            TestId = Guid.NewGuid()
        };

        var jobVacancyRequest = new CreateJobVacancyTestCommand
        {
            Id = guid,
            NumberOfQuestion = 1
        };

        _httpClientMock.Setup(x => x.SendTestCreationRequest(jobVacancyRequest)).ReturnsAsync(testInfo);

        var TestDbSetMock = new Mock<DbSet<Test>>();
        var newEntityGuid = Guid.NewGuid();
        TestDbSetMock.Setup(d => d.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>())).Callback<Test, CancellationToken>((t, ct) => t.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Tests).Returns(TestDbSetMock.Object);


        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        // Assert
    }
}
