using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;
using MRA.Jobs.Domain.Entities;
public class DeleteJobVacancyCommandHandlerTests : BaseTestFixture
{
    private DeleteJobVacancyCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
    //       _handler = new DeleteJobVacancyCommandHandler(_dbContextMock.Object);
    }

    [Test]
    public async Task Handle_JobVacancyExists_ShouldRemoveJobVacancy()
    {
        // Arrange
        var jobVacancy = new JobVacancy { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { jobVacancy.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(jobVacancy);

        var command = new DeleteJobVacancyCommand { Id = jobVacancy.Id };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.JobVacancies.Remove(jobVacancy), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_JobVacancyNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteJobVacancyCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as JobVacancy);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.JobVacancies.Remove(It.IsAny<JobVacancy>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}

