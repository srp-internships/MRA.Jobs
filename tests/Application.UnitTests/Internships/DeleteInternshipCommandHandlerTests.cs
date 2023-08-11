using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Delete;

namespace MRA.Jobs.Application.UnitTests.Internships;

public class DeleteInternshipCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteInternshipVacancyCommandHandler(_dbContextMock.Object);
    }

    private DeleteInternshipVacancyCommandHandler _handler;

    [Test]
    [Ignore("")]
    public async Task Handle_InternshipExists_ShouldRemoveInternship()
    {
        // Arrange
        InternshipVacancy internship = new InternshipVacancy { Id = Guid.NewGuid() };
        _dbContextMock
            .Setup(x => x.Internships.FindAsync(new object[] { internship.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(internship);

        DeleteInternshipVacancyCommand command = new DeleteInternshipVacancyCommand { Id = internship.Id };

        // Act
        bool result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.Internships.Remove(internship), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    [Ignore("")]
    public void Handle_InternshipNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        DeleteInternshipVacancyCommand command = new DeleteInternshipVacancyCommand { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.Internships.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as InternshipVacancy);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.Internships.Remove(It.IsAny<InternshipVacancy>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}