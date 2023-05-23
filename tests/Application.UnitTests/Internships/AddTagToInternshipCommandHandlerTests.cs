namespace MRA.Jobs.Application.UnitTests.Applicant;

using Azure;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.Internships.Command.Tags;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;
using MRA.Jobs.Domain.Entities;
[TestFixture]
public class AddTagToInternshipCommandHandlerTests : BaseTestFixture
{
    private AddTagToInternshipCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new AddTagToInternshipCommandHandler(
            _dbContextMock.Object,
            Mapper,            
            _dateTimeMock.Object,
            _currentUserServiceMock.Object
        );
    }

    [Test]
    public async Task Handle_AddsTagsInternship_ReturnsTrue()
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

        var tagDbSetMock1 = new Mock<DbSet<Tag>>();
        _dbContextMock.Setup(x => x.Tags).Returns(tagDbSetMock1.Object);
        var tagDbSetMock2 = new Mock<DbSet<Tag>>();
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
        tagDbSetMock2.Setup(x => x.FindAsync(new object[] { tag2.Id }, CancellationToken.None)).ReturnsAsync(tag2);



        var vacancyTagsDbSetMock = new Mock<DbSet<VacancyTag>>();
        _dbContextMock.Setup(x => x.VacancyTags).Returns(vacancyTagsDbSetMock.Object);

        var timelineEventSetMock = new Mock<DbSet<VacancyTimelineEvent>>();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timelineEventSetMock.Object);

        var timelineEvent = new VacancyTimelineEvent
        {
            VacancyId = Internship.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTimeMock.Object.Now,
            Note = $"Added '{tag1.Name}, {tag2.Name}' tag",
            CreateBy = _currentUserServiceMock.Object.UserId
        };
        timelineEventSetMock.Setup(x => x.FindAsync(new object[] { timelineEvent.Id }, CancellationToken.None)).ReturnsAsync(timelineEvent);


        var command = new AddTagToInternshipCommand
        {
            InternshipId = InternshipId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.IsTrue(result);
    }

 
}



