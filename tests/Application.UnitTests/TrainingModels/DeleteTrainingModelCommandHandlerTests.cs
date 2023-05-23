using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Features.TraningModels.Commands.DeleteTraningModel;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;
using MRA.Jobs.Domain.Entities;
public class DeleteTrainingModelCommandHandlerTests : BaseTestFixture
{
    private DeleteTrainingModelCommandHadler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new DeleteTrainingModelCommandHadler(_dbContextMock.Object);
    }

    [Test]
    public async Task Handle_TrainingModelExists_ShouldRemoveJobVacancy()
    {
        // Arrange
        var trainingModel = new TrainingModel { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.TrainingModels.FindAsync(new object[] { trainingModel.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(trainingModel);

        var command = new DeleteTrainingModelCommand { Id = trainingModel.Id };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.TrainingModels.Remove(trainingModel), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_JobVacancyNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteTrainingModelCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.TrainingModels.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TrainingModel);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.TrainingModels.Remove(It.IsAny<TrainingModel>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
