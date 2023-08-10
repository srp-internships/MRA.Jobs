using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicants.Command.DeleteApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class DeleteApplicantCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteApplicantCommandHandler(_dbContextMock.Object);
    }

    private DeleteApplicantCommandHandler _handler;

    [Test]
    public async Task Handle_ApplicantExists_ShouldRemoveApplicant()
    {
        // Arrange 
        Domain.Entities.Applicant applicant = new Domain.Entities.Applicant { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applicants
                .FindAsync(new object[] { applicant.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicant);

        DeleteApplicantCommand command = new DeleteApplicantCommand { Id = applicant.Id };

        // Act
        bool result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(a => a.Applicants.Remove(applicant), Times.Once);
        _dbContextMock.Verify(a => a.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_ApplicantNotFound_ShouldThrowNotFoundException()
    {
        // Arrange 
        DeleteApplicantCommand command = new DeleteApplicantCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(i => i.Applicants.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Domain.Entities.Applicant);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(i => i.Applicants.Remove(It.IsAny<Domain.Entities.Applicant>()), Times.Never);
        _dbContextMock.Verify(i => i.SaveChangesAsync(default), Times.Never);
    }
}