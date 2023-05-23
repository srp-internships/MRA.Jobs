namespace MRA.Jobs.Application.UnitTests.Applicant;

using Azure;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;
using MRA.Jobs.Domain.Entities;
[TestFixture]
public class AddTagToJobVacancyCommandHandlerTests : BaseTestFixture
{
    private AddTagToJobVacancyCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new AddTagToJobVacancyCommandHandler(
            _dbContextMock.Object,
            Mapper,
            _currentUserServiceMock.Object,
            _dateTimeMock.Object
        );
    }

    [Test]
    public async Task Handle_AddsTagsJobVacancy_ReturnsTrue()
    {
        // Arrange
        var jobVacancyId = Guid.NewGuid();

        var jobVacancy = new JobVacancy
        {
            Id = jobVacancyId,
        };

        var jobVacancyDbSetMock = new Mock<DbSet<JobVacancy>>();

        _dbContextMock.Setup(x => x.JobVacancies).Returns(jobVacancyDbSetMock.Object);

        jobVacancyDbSetMock.Setup(x => x.FindAsync(new object[] { jobVacancyId }, CancellationToken.None))
            .ReturnsAsync(jobVacancy);

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
            VacancyId = jobVacancy.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTimeMock.Object.Now,
            Note = $"Added '{tag1.Name}, {tag2.Name}' tag",
            CreateBy = _currentUserServiceMock.Object.UserId
        };
        timelineEventSetMock.Setup(x => x.FindAsync(new object[] { timelineEvent.Id }, CancellationToken.None)).ReturnsAsync(timelineEvent);


        var command = new AddTagsToJobVacancyCommand
        {
            JobVacancyId = jobVacancyId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.IsTrue(result);
    }
}



