﻿namespace MRA.Jobs.Application.UnitTests.TrainingModels;

using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Domain.Entities;
public class DeleteTrainingModelCommandHandlerTests : BaseTestFixture
{
    private DeleteTrainingVacancyCommandHadler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new DeleteTrainingVacancyCommandHadler(_dbContextMock.Object);
    }

    [Test]
    public async Task Handle_TrainingModelExists_ShouldRemoveJobVacancy()
    {
        // Arrange
        var trainingModel = new TrainingVacancy { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.TrainingVacancies.FindAsync(new object[] { trainingModel.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(trainingModel);

        var command = new DeleteTrainingVacancyCommand { Id = trainingModel.Id };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.TrainingVacancies.Remove(trainingModel), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_JobVacancyNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteTrainingVacancyCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.TrainingVacancies.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TrainingVacancy);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.TrainingVacancies.Remove(It.IsAny<TrainingVacancy>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
