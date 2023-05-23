using System.Linq.Expressions;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Applicant;
using MRA.Jobs.Domain.Entities;
public class RemoveTagsFromApplicantCommandHandlerTests : BaseTestFixture
{
    private RemoveTagsFromApplicantCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new RemoveTagsFromApplicantCommandHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    public async Task Handle_RemovesTagsFromApplicant_ReturnsTrue()
    {
        // Arrange
        var applicantId = Guid.NewGuid();

        var applicant = new Applicant
        {
            Id = applicantId,
        };

        var applicantDbSetMock = new Mock<DbSet<Applicant>>();

        _dbContextMock.Setup(x => x.Applicants).Returns(applicantDbSetMock.Object);

        applicantDbSetMock.Setup(x => x.FindAsync(new object[] { applicantId }, CancellationToken.None))
            .ReturnsAsync(applicant);

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
            UserId = applicantId,
            TagId = tag1.Id,
            Tag = tag1
        };
        var userTag2 = new UserTag
        {
            UserId = applicantId,
            TagId = tag2.Id,
            Tag = tag2
        };

        var userTagsDbSetMock = new Mock<DbSet<UserTag>>();
        _dbContextMock.Setup(x => x.UserTags).Returns(userTagsDbSetMock.Object);

        userTagsDbSetMock.Setup(x => x.FindAsync(new object[] { applicantId, tag1.Name }, CancellationToken.None)).ReturnsAsync(userTag1);
        userTagsDbSetMock.Setup(x => x.FindAsync(new object[] { applicantId, tag2.Name }, CancellationToken.None)).ReturnsAsync(userTag2);

        var command = new RemoveTagsFromApplicantCommand
        {
            ApplicantId = applicantId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userTagsDbSetMock.Verify(x => x.Remove(It.Is<UserTag>(ut => ut.UserId == applicantId && ut.TagId == tag1.Id)), Times.Once);
        userTagsDbSetMock.Verify(x => x.Remove(It.Is<UserTag>(ut => ut.UserId == applicantId && ut.TagId == tag2.Id)), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.IsTrue(result);  
    }
}

