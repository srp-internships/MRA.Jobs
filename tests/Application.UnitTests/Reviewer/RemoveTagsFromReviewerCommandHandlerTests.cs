using System.Linq.Expressions;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;
using MRA.Jobs.Application.Features.Reviewer.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
using MRA.Jobs.Domain.Entities;
public class RemoveTagsFromReviewerCommandHandlerTests : BaseTestFixture
{
    private RemoveTagsFromReviewerCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new RemoveTagsFromReviewerCommandHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    public async Task Handle_RemovesTagsFromReviewer_ReturnsTrue()
    {
        // Arrange
        var ReviewerId = Guid.NewGuid();

        var Reviewer = new Reviewer
        {
            Id = ReviewerId,
        };

        var ReviewerDbSetMock = new Mock<DbSet<Reviewer>>();

        _dbContextMock.Setup(x => x.Reviewers).Returns(ReviewerDbSetMock.Object);

        ReviewerDbSetMock.Setup(x => x.FindAsync(new object[] { ReviewerId }, CancellationToken.None))
            .ReturnsAsync(Reviewer);

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

        var userTag1 = new UserTag
        {
            UserId = ReviewerId,
            TagId = tag1.Id,
            Tag = tag1
        };
        var userTag2 = new UserTag
        {
            UserId = ReviewerId,
            TagId = tag2.Id,
            Tag = tag2
        };

        var userTagsDbSetMock = new Mock<DbSet<UserTag>>();
        _dbContextMock.Setup(x => x.UserTags).Returns(userTagsDbSetMock.Object);

        userTagsDbSetMock.Setup(x => x.FindAsync(new object[] { ReviewerId, tag1.Name }, CancellationToken.None)).ReturnsAsync(userTag1);
        userTagsDbSetMock.Setup(x => x.FindAsync(new object[] { ReviewerId, tag2.Name }, CancellationToken.None)).ReturnsAsync(userTag2);

        var command = new RemoveTagsFromReviewerCommand
        {
            ReviewerId = ReviewerId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userTagsDbSetMock.Verify(x => x.Remove(It.Is<UserTag>(ut => ut.UserId == ReviewerId && ut.TagId == tag1.Id)), Times.Once);
        userTagsDbSetMock.Verify(x => x.Remove(It.Is<UserTag>(ut => ut.UserId == ReviewerId && ut.TagId == tag2.Id)), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.IsTrue(result);  
    }
}

