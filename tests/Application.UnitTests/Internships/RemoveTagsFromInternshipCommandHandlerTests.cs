namespace MRA.Jobs.Application.UnitTests.Internship;

using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Features.Internships.Command.Tags;
using MRA.Jobs.Domain.Entities;
public class RemoveTagsFromInternshipCommandHandlerTests : BaseTestFixture
{
    private RemoveTagFromInternshipCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new RemoveTagFromInternshipCommandHandler(_dbContextMock.Object,
            Mapper,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object);
    }

    [Test]
    public async Task Handle_RemovesTagsFromInternship_ReturnsTrue()
    {
        // Arrange
        var InternshipId = Guid.NewGuid();

        var Internship = new Internship
        {
            Id = InternshipId,
        };

        var InternshipDbSetMock = new Mock<DbSet<Internship>>();

        _dbContextMock.Setup(x => x.Internships).Returns(InternshipDbSetMock.Object);

        InternshipDbSetMock.Setup(x => x.FindAsync(new object[] { InternshipId }, CancellationToken.None))
            .ReturnsAsync(Internship);

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

        var vacancyTag1 = new VacancyTag
        {
            VacancyId = InternshipId,
            TagId = tag1.Id,
            Tag = tag1
        };
        var vacancyTag2 = new VacancyTag
        {
            VacancyId = InternshipId,
            TagId = tag2.Id,
            Tag = tag2
        };

        var vacancyTagsDbSetMock = new Mock<DbSet<VacancyTag>>();
        _dbContextMock.Setup(x => x.VacancyTags).Returns(vacancyTagsDbSetMock.Object);

        vacancyTagsDbSetMock.Setup(x => x.FindAsync(new object[] { InternshipId, tag1.Name }, CancellationToken.None)).ReturnsAsync(vacancyTag1);
        vacancyTagsDbSetMock.Setup(x => x.FindAsync(new object[] { InternshipId, tag2.Name }, CancellationToken.None)).ReturnsAsync(vacancyTag2);

        var command = new RemoveTagFromInternshipCommand
        {
            InternshipId = InternshipId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        vacancyTagsDbSetMock.Verify(x => x.Remove(It.Is<VacancyTag>(ut => ut.VacancyId == InternshipId && ut.TagId == tag1.Id)), Times.Once);
        vacancyTagsDbSetMock.Verify(x => x.Remove(It.Is<VacancyTag>(ut => ut.VacancyId == InternshipId && ut.TagId == tag2.Id)), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.IsTrue(result);
    }
}

