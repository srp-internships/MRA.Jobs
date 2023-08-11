using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Delete;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

public class DeleteTrainingModelCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new DeleteTrainingVacancyCommandHadler(_dbContextMock.Object);
    }

    private DeleteTrainingVacancyCommandHadler _handler;

    [Test]
    public async Task Handle_TrainingModelExists_ShouldRemoveJobVacancy()
    {
        // Arrange
        TrainingVacancy trainingModel = new TrainingVacancy { Id = Guid.NewGuid() };
        _dbContextMock
            .Setup(x => x.TrainingVacancies.FindAsync(new object[] { trainingModel.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trainingModel);

        DeleteTrainingVacancyCommand command = new DeleteTrainingVacancyCommand { Id = trainingModel.Id };

        // Act
        bool result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.TrainingVacancies.Remove(trainingModel), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_JobVacancyNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        DeleteTrainingVacancyCommand command = new DeleteTrainingVacancyCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x =>
                x.TrainingVacancies.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TrainingVacancy);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.TrainingVacancies.Remove(It.IsAny<TrainingVacancy>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}