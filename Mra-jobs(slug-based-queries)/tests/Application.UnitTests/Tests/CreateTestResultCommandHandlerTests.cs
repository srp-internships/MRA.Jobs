﻿using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Features.Tests.Commands.CreateTestResult;

namespace MRA.Jobs.Application.UnitTests.Tests;
public class CreateTestResultCommandHandlerTests : BaseTestFixture
{
    private CreateTestResultCommandHandler _handler;
    private Mock<ITestHttpClientService> _httpClientMock;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _httpClientMock = new Mock<ITestHttpClientService>();
        _handler = new CreateTestResultCommandHandler(
            _dateTimeMock.Object,
            _currentUserServiceMock.Object,
            _httpClientMock.Object,
            _dbContextMock.Object);
    }

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateTestResult()
    {
        // Arrange
        var testResult = new TestResultDTO
        {
            Score = 1,
            TestId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        var request = new CreateTestResultCommand
        {
            Score = testResult.Score,
            TestId = testResult.TestId,
            UserId = testResult.UserId
        };

        _httpClientMock.Setup(x => x.GetTestResultRequest(request)).ReturnsAsync(testResult);

        var testResultDbSetMock = new Mock<DbSet<TestResult>>();
        var newEntityGuid = Guid.NewGuid();
        testResultDbSetMock.Setup(d => d.AddAsync(It.IsAny<TestResult>(), It.IsAny<CancellationToken>())).Callback<TestResult, CancellationToken>((t, ct) => t.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.TestResults).Returns(testResultDbSetMock.Object);

        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(testResult);

        testResultDbSetMock.Verify(x => x.AddAsync(It.Is<TestResult>(t =>
            t.Score == request.Score &&
            t.CreatedBy == request.UserId &&
            t.TestId == request.TestId
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
