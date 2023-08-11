using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.DeleteApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;

public class DeleteApplicationCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteApplicationCommandHandler(_dbContextMock.Object, _dateTimeMock.Object,
            _currentUserServiceMock.Object);
    }

    private DeleteApplicationCommandHandler _handler;

    [Test]
    public async Task Handle_ApplicationExists_ShouldRemoveApplication()
    {
        // Arrange
        Domain.Entities.Application application = new Domain.Entities.Application { Id = Guid.NewGuid() };
        _dbContextMock
            .Setup(x => x.Applications.FindAsync(new object[] { application.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(application);

        DeleteApplicationCommand command = new DeleteApplicationCommand { Id = application.Id };

        // Act
        bool result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.Applications.Remove(application), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_ApplicationNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        DeleteApplicationCommand command = new DeleteApplicationCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.Applications.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Domain.Entities.Application);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.Applications.Remove(It.IsAny<Domain.Entities.Application>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}