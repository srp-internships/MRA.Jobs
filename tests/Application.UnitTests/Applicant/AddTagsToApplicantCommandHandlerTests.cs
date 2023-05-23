namespace MRA.Jobs.Application.UnitTests.Applicant;

using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.Tags;
using MRA.Jobs.Domain.Entities;
[TestFixture]
public class AddTagToApplicantCommandHandlerTests : BaseTestFixture
{
    private AddTagToApplicantCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new AddTagToApplicantCommandHandler(
            _dbContextMock.Object,
            Mapper
        );
    }
    [Test]
    public async Task Handle_AddsTagsToApplicant_ReturnsTrue()
    {
        // Arrange
        var ApplicantId = Guid.NewGuid();

        var Applicant = new Applicant
        {
            Id = ApplicantId,
        };

        var ApplicantDbSetMock = new Mock<DbSet<Applicant>>();

        _dbContextMock.Setup(x => x.Applicants).Returns(ApplicantDbSetMock.Object);

        ApplicantDbSetMock.Setup(x => x.FindAsync(new object[] { ApplicantId }, CancellationToken.None))
            .ReturnsAsync(Applicant);

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

        var userTagsDbSetMock = new Mock<DbSet<UserTag>>();
        _dbContextMock.Setup(x => x.UserTags).Returns(userTagsDbSetMock.Object);

        var command = new AddTagsToApplicantCommand
        {
            ApplicantId = ApplicantId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.IsTrue(result);
    }
}


