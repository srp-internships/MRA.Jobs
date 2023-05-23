namespace MRA.Jobs.Application.UnitTests.Applicant;

using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Domain.Entities;
using MRA.Jobs.Application.Features.TraningModels.Commands.Tags;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

[TestFixture]
public class AddTagToTrainingModelCommandHandlerTests : BaseTestFixture
{
    private AddTagToTrainingModelCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new AddTagToTrainingModelCommandHandler(
            _dbContextMock.Object,
            Mapper,            
            _dateTimeMock.Object,
            _currentUserServiceMock.Object
        );
    }

    [Test]
    public async Task Handle_AddsTagsTraining_ReturnsTrue()
    {
        // Arrange
        var TrainingId = Guid.NewGuid();

        var training = new TrainingModel
        {
            Id = TrainingId,
        };

        var TrainingDbSetMock = new Mock<DbSet<TrainingModel>>();

        _dbContextMock.Setup(x => x.TrainingModels).Returns(TrainingDbSetMock.Object);

        TrainingDbSetMock.Setup(x => x.FindAsync(new object[] { TrainingId }, CancellationToken.None))
            .ReturnsAsync(training);

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
            VacancyId = training.Id,
            EventType = TimelineEventType.Created,
            Time = _dateTimeMock.Object.Now,
            Note = $"Added '{tag1.Name}, {tag2.Name}' tag",
            CreateBy = _currentUserServiceMock.Object.UserId
        };
        timelineEventSetMock.Setup(x => x.FindAsync(new object[] { timelineEvent.Id }, CancellationToken.None)).ReturnsAsync(timelineEvent);


        var command = new AddTagToTrainingModelCommand
        {
            TrainingModelId = TrainingId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        Assert.IsTrue(result);
    }

 
}



