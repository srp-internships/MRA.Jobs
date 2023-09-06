using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Delete;

namespace MRA.Jobs.Application.UnitTests.Internships;
using MRA.Jobs.Domain.Entities;
public class DeleteInternshipCommandHandlerTests : BaseTestFixture
{
    DeleteInternshipVacancyCommandHandler _handler;
    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteInternshipVacancyCommandHandler(_dbContextMock.Object);
    }

    [Test]
    [Ignore("")]
    public async Task Handle_InternshipExists_ShouldRemoveInternship()
    {
        // Arrange
        var internship = new InternshipVacancy { Slug = string.Empty };
        _dbContextMock.Setup(x => x.InternshipVacancies.FindAsync(new object[] { internship.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(internship);

        var command = new DeleteInternshipVacancyCommand { Slug = internship.Slug };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(x => x.InternshipVacancies.Remove(internship), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    [Ignore("")]
    public void Handle_InternshipNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteInternshipVacancyCommand { Slug = string.Empty };

        _dbContextMock.Setup(x => x.InternshipVacancies.FindAsync(new object[] { command.Slug }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as InternshipVacancy);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(x => x.InternshipVacancies.Remove(It.IsAny<InternshipVacancy>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
