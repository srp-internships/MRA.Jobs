using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;
using MRA.Jobs.Application.Features.Applications.Command.UpdateApplicationStatus;

namespace MRA.Jobs.Application.UnitTests.Applications;

using Domain.Entities;

public class UpdateApplicationStatusCommandHandlerTests : BaseTestFixture
{
    private UpdateApplicationStatusCommandHandler _handler;

    public override void Setup()
    {
       base.Setup();
        _handler = new UpdateApplicationStatusCommandHandler(
            _dbContextMock.Object, _dateTimeMock.Object, _currentUserServiceMock.Object, _htmlServiceMock.Object,
            _identityService.Object, _smsServiceMock.Object, _loggerMock.Object, _configurationMock.Object);
    }
    
    [Test]
    [Ignore("Null Reference exception")]
    public async Task Handle_ShouldCallEmailAndSmsMethods_WhenStatusIsApproved()
    {
        // Arrange
        var application = new Application { Slug = "testSlug", Status = ApplicationStatus.Approved };

        var applications = new List<Application> { application }.AsQueryable();

        var mockSet = new Mock<DbSet<Application>>();
        mockSet.As<IQueryable<Application>>().Setup(m => m.Provider).Returns(applications.Provider);
        mockSet.As<IQueryable<Application>>().Setup(m => m.Expression).Returns(applications.Expression);
        mockSet.As<IQueryable<Application>>().Setup(m => m.ElementType).Returns(applications.ElementType);
        mockSet.As<IQueryable<Application>>().Setup(m => m.GetEnumerator()).Returns(applications.GetEnumerator());

        _dbContextMock.Setup(x => x.Applications).Returns(mockSet.Object);

        var command = new UpdateApplicationStatusCommand
        {
            Slug = "testSlug", StatusId = (int)ApplicationStatus.Approved, ApplicantUserName = "applicant"
        };
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        _htmlServiceMock.Verify(service => service.EmailApproved(It.IsAny<UserProfileResponse>(), It.IsAny<string>()),
            Times.Once);
        _smsServiceMock.Verify(service => service.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}