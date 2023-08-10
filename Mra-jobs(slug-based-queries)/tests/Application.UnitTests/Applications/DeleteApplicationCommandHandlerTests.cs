﻿using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.DeleteApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;
using MRA.Jobs.Domain.Entities;


public class DeleteApplicationCommandHandlerTests : BaseTestFixture
{
    private DeleteApplicationCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteApplicationCommandHandler(_dbContextMock.Object, _dateTimeMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public async Task Handle_ApplicationExists_ShouldRemoveApplication()
    {
        // Arrange
        var application = new Application { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applications.FindAsync(new object[] { application.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(application);

        var command = new DeleteApplicationCommand { Id = application.Id };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.Applications.Remove(application), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_ApplicationNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteApplicationCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.Applications.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Application);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.Applications.Remove(It.IsAny<Application>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
