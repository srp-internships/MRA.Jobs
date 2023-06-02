using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancyTest;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;
public class CreateJobVacancyTestCommandHandlerTests : BaseTestFixture
{
    private CreateJobVacancyTestCommandHandler _handler;
    private Mock<ITestHttpClientService> _httpClientMock;


    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _httpClientMock = new Mock<ITestHttpClientService>();
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
        var testInfo = new TestInfoDTO
        {
            MaxScore = 10,
            TestId = Guid.NewGuid()
        };

        var jobVacancyRequest = new CreateJobVacancyTestCommand
        {
            Id = Guid.NewGuid(),
            NumberOfQuestion = 0
        };

        _httpClientMock.Setup(x => x.SendTestCreationRequest(jobVacancyRequest)).ReturnsAsync(testInfo);

        var testDbSetMock = new Mock<DbSet<Test>>();
        var newEntityGuid = Guid.NewGuid();
        testDbSetMock.Setup(d => d.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>())).Callback<Test, CancellationToken>((t, ct) => t.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Tests).Returns(testDbSetMock.Object);

        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(jobVacancyRequest, CancellationToken.None);

        // Assert
        result.Should().Be(testInfo);

        testDbSetMock.Verify(x => x.AddAsync(It.Is<Test>(t =>
            t.VacancyId == jobVacancyRequest.Id &&
            t.NumberOfQuestion == jobVacancyRequest.NumberOfQuestion
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}