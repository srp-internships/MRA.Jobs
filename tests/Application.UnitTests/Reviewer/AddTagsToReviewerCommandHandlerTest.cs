using MRA.Jobs.Application.Contracts.Reviewer.Commands;
using MRA.Jobs.Application.Features.Reviewer.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
using MRA.Jobs.Domain.Entities;

[TestFixture]
public class AddTagToReviewerCommandHandlerTests : BaseTestFixture
{
    private AddTagToReviewerCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new AddTagToReviewerCommandHandler(
            _dbContextMock.Object,
            Mapper
        );
    }
    [Test]
    public async Task Handle_AddsTagsToReviewer_ReturnsTrue()
    {
        // Arrange
        var reviewerId = Guid.NewGuid();

        var reviewer = new Reviewer
        {
            Id = reviewerId,
        };

        var reviewerDbSetMock = new Mock<DbSet<Reviewer>>();

        _dbContextMock.Setup(x => x.Reviewers).Returns(reviewerDbSetMock.Object);

        reviewerDbSetMock.Setup(x => x.FindAsync(new object[] { reviewerId }, CancellationToken.None))
            .ReturnsAsync(reviewer);

        var tagDbSetMock1 = new Mock<DbSet<Tag>>();
        _dbContextMock.Setup(x => x.Tags).Returns(tagDbSetMock1.Object);

        var tag1 = new Tag
        {
            Id = Guid.NewGuid(),
            Name = "Tag1"
        };
        var tag2 = new Tag
        {
            Id = Guid.NewGuid(),
            Name = "Tag2"
        };
        tagDbSetMock1.Setup(x => x.FindAsync(new object[] { tag1.Id }, CancellationToken.None)).ReturnsAsync(tag1);
        tagDbSetMock1.Setup(x => x.FindAsync(new object[] { tag2.Id }, CancellationToken.None)).ReturnsAsync(tag2);

        var userTagsDbSetMock= new Mock<DbSet<UserTag>>();
        _dbContextMock.Setup(x => x.UserTags).Returns(userTagsDbSetMock.Object);

        var command = new AddTagsToReviewerCommand
        {
            ReviewerId = reviewerId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.IsTrue(result);
    }
}
