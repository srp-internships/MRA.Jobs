using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.DeleteReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
using Domain.Entities;

public class DeleteReviewerCommandHandlerTests : BaseTestFixture
{
    private DeleteReviewerCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteReviewerCommandHandler(_dbContextMock.Object);
    }

    [Test]
    public async Task Handle_ReviewerExists_ShouldRemoveApplicant()
    {
        // Arrange 
        var reviewer = new Reviewer { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Reviewers
                .FindAsync(new object[] { reviewer.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reviewer);

        var command = new DeleteReviewerCommand { Id = reviewer.Id };
        
        // Act
        var result = await _handler.Handle(command, default);
        
        // Assert
        _dbContextMock.Verify(a => a.Reviewers.Remove(reviewer), Times.Once);
        _dbContextMock.Verify(a => a.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_ReviewerNotFound_ShouldThrowNotFoundException()
    {
        // Arrange 
        var command = new DeleteReviewerCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(i => i.Reviewers.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Reviewer);
        
        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(i => i.Reviewers.Remove(It.IsAny<Reviewer>()), Times.Never);
        _dbContextMock.Verify(i => i.SaveChangesAsync(default), Times.Never);
    }
    
}