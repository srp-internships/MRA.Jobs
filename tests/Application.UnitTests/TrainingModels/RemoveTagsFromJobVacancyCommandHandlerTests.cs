namespace MRA.Jobs.Application.UnitTests.TrainingModel;

using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;
using MRA.Jobs.Application.Features.TrainingModels.Commands.Tags;
using MRA.Jobs.Domain.Entities;
public class RemoveTagsFromTrainingModelCommandHandlerTests : BaseTestFixture
{
    private RemoveTagFromTrainingModelCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new RemoveTagFromTrainingModelCommandHandler(_dbContextMock.Object,
            Mapper,
            _currentUserServiceMock.Object,
            _dateTimeMock.Object);
    }

    [Test]
    public async Task Handle_RemovesTagsFromTrainingModel_ReturnsTrue()
    {
        // Arrange
        var TrainingModelId = Guid.NewGuid();

        var TrainingModel = new TrainingModel
        {
            Id = TrainingModelId,
        };

        var trainingModelDbSetMock = new Mock<DbSet<TrainingModel>>();

        _dbContextMock.Setup(x => x.TrainingModels).Returns(trainingModelDbSetMock.Object);

        trainingModelDbSetMock.Setup(x => x.FindAsync(new object[] { TrainingModelId }, CancellationToken.None))
            .ReturnsAsync(TrainingModel);

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
            VacancyId = TrainingModelId,
            TagId = tag1.Id,
            Tag = tag1
        };
        var vacancyTag2 = new VacancyTag
        {
            VacancyId = TrainingModelId,
            TagId = tag2.Id,
            Tag = tag2
        };

        var vacancyTagsDbSetMock = new Mock<DbSet<VacancyTag>>();
        _dbContextMock.Setup(x => x.VacancyTags).Returns(vacancyTagsDbSetMock.Object);

        vacancyTagsDbSetMock.Setup(x => x.FindAsync(new object[] { TrainingModelId, tag1.Name }, CancellationToken.None)).ReturnsAsync(vacancyTag1);
        vacancyTagsDbSetMock.Setup(x => x.FindAsync(new object[] { TrainingModelId, tag2.Name }, CancellationToken.None)).ReturnsAsync(vacancyTag2);

        var command = new RemoveTagFromTrainingModelCommand
        {
            TrainingModelId = TrainingModelId,
            Tags = new string[] { tag1.Name, tag2.Name }
        };

        var timeLineEventDbSetMock = new Mock<DbSet<VacancyTimelineEvent>>();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timeLineEventDbSetMock.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        vacancyTagsDbSetMock.Verify(x => x.Remove(It.Is<VacancyTag>(ut => ut.VacancyId == TrainingModelId && ut.TagId == tag1.Id)), Times.Once);
        vacancyTagsDbSetMock.Verify(x => x.Remove(It.Is<VacancyTag>(ut => ut.VacancyId == TrainingModelId && ut.TagId == tag2.Id)), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        Assert.IsTrue(result);
    }
}

