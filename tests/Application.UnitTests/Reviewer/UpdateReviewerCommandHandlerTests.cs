using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
using Domain.Entities;

public class UpdateReviewerCommandHandlerTests : BaseTestFixture
{
    private UpdateReviewerCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new UpdateReviewerCommandHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    public void Handle_GivenNonExistentReviewerId_ShouldThrowNotFoundException()
    {
        // Arrange 
        var command = new UpdateReviewerCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Reviewers.FindAsync(command.Id))
            .ReturnsAsync(null as Reviewer);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert 
        act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Reviewer)}*{command.Id}*");
    }

}