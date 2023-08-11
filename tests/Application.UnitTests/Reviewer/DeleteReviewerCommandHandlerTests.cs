using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.DeleteReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class DeleteReviewerCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new DeleteReviewerCommandHandler(_dbContextMock.Object);
    }

    private DeleteReviewerCommandHandler _handler;

    [Test]
    public async Task Handle_ReviewerExists_ShouldRemoveApplicant()
    {
        // Arrange 
        Domain.Entities.Reviewer reviewer = new Domain.Entities.Reviewer { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Reviewers
                .FindAsync(new object[] { reviewer.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reviewer);

        DeleteReviewerCommand command = new DeleteReviewerCommand { Id = reviewer.Id };

        // Act
        bool result = await _handler.Handle(command, default);

        // Assert
        _dbContextMock.Verify(a => a.Reviewers.Remove(reviewer), Times.Once);
        _dbContextMock.Verify(a => a.SaveChangesAsync(default), Times.Once);
        Assert.True(result);
    }

    [Test]
    public void Handle_ReviewerNotFound_ShouldThrowNotFoundException()
    {
        // Arrange 
        DeleteReviewerCommand command = new DeleteReviewerCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(i => i.Reviewers.FindAsync(new object[] { command.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Domain.Entities.Reviewer);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        _dbContextMock.Verify(i => i.Reviewers.Remove(It.IsAny<Domain.Entities.Reviewer>()), Times.Never);
        _dbContextMock.Verify(i => i.SaveChangesAsync(default), Times.Never);
    }
}