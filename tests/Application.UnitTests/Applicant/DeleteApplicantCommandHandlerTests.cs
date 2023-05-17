using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.DeleteApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;
using Domain.Entities;

public class DeleteApplicantCommandHandlerTests : BaseTestFixture
{
    private DeleteApplicantCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteApplicantCommandHandler(_dbContextMock.Object);
    }

    [Test]
    public async Task Handle_ApplicantExists_ShouldRemoveApplicant()
    {
        // Arrange 
        var applicant = new Applicant { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applicants
            .FindAsync(new object[] { applicant.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicant);

        var command = new DeleteApplicantCommand { Id = applicant.Id };
        
        // Act
        var result = await _handler.Handle(command, default);
        
        // Assert
        _dbContextMock.Verify(a => a.Applicants.Remove(applicant), Times.Once);
        _dbContextMock.Verify(a => a.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_ApplicantNotFound_ShouldThrowNotFoundException()
    {
        // Arrange 
        var command = new DeleteApplicantCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(i => i.Applicants.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Applicant);
        
        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(i => i.Applicants.Remove(It.IsAny<Applicant>()), Times.Never);
        _dbContextMock.Verify(i => i.SaveChangesAsync(default), Times.Never);
    }
}