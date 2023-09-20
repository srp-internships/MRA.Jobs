using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands.CreateTest;
using MRA.Jobs.Application.Features.Tests.Commands.CreateTest;

namespace MRA.Jobs.Application.UnitTests.Tests;
public class CreateTestCommandHandlerTests : BaseTestFixture
{
    private CreateTestCommandHandler _handler;
    private Mock<ITestHttpClientService> _httpClientMock;


    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _httpClientMock = new Mock<ITestHttpClientService>();
        _handler = new CreateTestCommandHandler(
            _dbContextMock.Object,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object,
            _httpClientMock.Object);
    }

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateTest()
    {
        // Arrange
        var testInfo = new TestInfoDto
        {
            MaxScore = 10,
            TestId = Guid.NewGuid()
        };

        var request = new CreateTestCommand
        {
            Slug = string.Empty,
            NumberOfQuestion = 0
        };

        _httpClientMock.Setup(x => x.SendTestCreationRequest(request)).ReturnsAsync(testInfo);

        var testDbSetMock = new Mock<DbSet<Test>>();
        var newEntityGuid = Guid.NewGuid();
        testDbSetMock.Setup(d => d.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>())).Callback<Test, CancellationToken>((t, ct) => t.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Tests).Returns(testDbSetMock.Object);

        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(testInfo);

        testDbSetMock.Verify(x => x.AddAsync(It.Is<Test>(t =>
            t.VacancyId == request.Id &&
            t.NumberOfQuestion == request.NumberOfQuestion
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}