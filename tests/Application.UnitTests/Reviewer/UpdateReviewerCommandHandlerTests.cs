using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class UpdateReviewerCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new UpdateReviewerCommandHandler(_dbContextMock.Object, Mapper);
    }

    private UpdateReviewerCommandHandler _handler;

    [Test]
    public void Handle_GivenNonExistentReviewerId_ShouldThrowNotFoundException()
    {
        // Arrange 
        UpdateReviewerCommand command = new UpdateReviewerCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Reviewers.FindAsync(command.Id))
            .ReturnsAsync(null as Domain.Entities.Reviewer);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert 
        act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Domain.Entities.Reviewer)}*{command.Id}*");
    }
}