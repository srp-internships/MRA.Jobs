using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Update;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;
public class UpdateTrainingModelCommandHandlerTests : BaseTestFixture
{
    private UpdateTrainingVacancyCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new UpdateTrainingVacancyCommandHandler(
            _dbContextMock.Object,
            Mapper,
            _currentUserServiceMock.Object,
            _dateTimeMock.Object,
            _slugGenerator.Object);
    }

    [Test]
    public void Handle_GivenNonExistentTrainingModelId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Slug = string.Empty };
        _dbContextMock.Setup(x => x.TrainingVacancies.FindAsync(command.Slug)).ReturnsAsync(null as TrainingVacancy);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(TrainingVacancy)}*{command.CategoryId}*");
    }

    [Test]
    [Ignore("")]
    public void Handle_GivenNonExistentCategoryId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Slug = string.Empty, CategoryId = Guid.Empty };
        _dbContextMock.Setup(x => x.TrainingVacancies.FindAsync(new object[] { command.Slug }, CancellationToken.None)).ReturnsAsync(new TrainingVacancy());
        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { command.CategoryId }, CancellationToken.None)).ReturnsAsync(null as VacancyCategory);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    [Ignore("")]
    public async Task Handle_GivenValidCommand_ShouldUpdateInternshipAndAddTimelineEvent()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand
        {
            Slug = string.Empty,
            Title = "Training Model Title",
            ShortDescription = "New Short Description",
            Description = "New Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.Empty,
            Duration = 10,
            Fees = 100
        };

        var trainingModelsDbSetMock = new Mock<DbSet<TrainingVacancy>>();
        var categoryDbSetMock = new Mock<DbSet<VacancyCategory>>();

        _dbContextMock.Setup(c => c.TrainingVacancies).Returns(trainingModelsDbSetMock.Object);
        _dbContextMock.Setup(c => c.Categories).Returns(categoryDbSetMock.Object);

        var trainingModel = new TrainingVacancy { Slug = command.Slug };
        trainingModelsDbSetMock.Setup(x => x.FindAsync(new object[] { command.Slug }, CancellationToken.None))
            .ReturnsAsync(trainingModel);

        var category = new VacancyCategory { Id = command.CategoryId };
        categoryDbSetMock.Setup(x => x.FindAsync(new object[] { command.CategoryId }, CancellationToken.None))
            .ReturnsAsync(category);

        var timelineEvent = new VacancyTimelineEvent();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents.AddAsync(It.IsAny<VacancyTimelineEvent>(), It.IsAny<CancellationToken>()))
            .Callback<VacancyTimelineEvent, CancellationToken>((vte, ct) => timelineEvent = vte);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(command.Slug);

        timelineEvent.Should().NotBeNull();

        timelineEvent.VacancyId.Should().Be(command.Slug);
        timelineEvent.EventType.Should().Be(TimelineEventType.Updated);
        timelineEvent.Time.Should().Be(_dateTimeMock.Object.Now);
        timelineEvent.Note.Should().Be("Training Model updated");
        timelineEvent.CreateBy.Should().Be(_currentUserServiceMock.Object.GetId().Value);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        trainingModel.Title.Should().Be(command.Title);
        trainingModel.ShortDescription.Should().Be(command.ShortDescription);
        trainingModel.Description.Should().Be(command.Description);
        trainingModel.PublishDate.Should().Be(command.PublishDate);
        trainingModel.EndDate.Should().Be(command.EndDate);
        trainingModel.CategoryId.Should().Be(command.CategoryId);
        trainingModel.Duration.Should().Be(command.Duration);
        trainingModel.Fees.Should().Be(command.Fees);
    }
}
