using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.UpdateJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class UpdateJobVacancyCommandHandlerTests : BaseTestFixture
{
    private UpdateJobVacancyCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new UpdateJobVacancyCommandHandler(
            _dbContextMock.Object, Mapper, _dateTimeMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public void Handle_GivenNonExistentJobVacancyId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(command.Id)).ReturnsAsync(null as JobVacancy);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(JobVacancy)}*{command.CategoryId}*");
    }

    [Test]
    public void Handle_GivenNonExistentCategoryId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand { Id = Guid.NewGuid(), CategoryId = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { command.Id }, CancellationToken.None)).ReturnsAsync(new JobVacancy());
        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { command.CategoryId }, CancellationToken.None)).ReturnsAsync(null as VacancyCategory);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Handle_GivenValidCommand_ShouldUpdateJobVacancyAndAddTimelineEvent()
    {
        // Arrange
        var command = new UpdateJobVacancyCommand
        {
            Id = Guid.NewGuid(),
            Title = "New Job Title",
            ShortDescription = "New Short Description",
            Description = "New Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 1,
            WorkSchedule = WorkSchedule.FullTime
        };

        var jobVacancyDbSetMock = new Mock<DbSet<JobVacancy>>();
        var categoryDbSetMock = new Mock<DbSet<VacancyCategory>>();

        _dbContextMock.Setup(x => x.JobVacancies).Returns(jobVacancyDbSetMock.Object);
        _dbContextMock.Setup(x => x.Categories).Returns(categoryDbSetMock.Object);

        var existingJobVacancy = new JobVacancy { Id = command.Id };
        jobVacancyDbSetMock.Setup(x => x.FindAsync(new object[] { command.Id }, CancellationToken.None))
            .ReturnsAsync(existingJobVacancy);

        var category = new VacancyCategory { Id = command.CategoryId };
        categoryDbSetMock.Setup(x => x.FindAsync(new object[] { command.CategoryId }, CancellationToken.None))
            .ReturnsAsync(category);

        var timelineEvent = new VacancyTimelineEvent();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents.AddAsync(It.IsAny<VacancyTimelineEvent>(), It.IsAny<CancellationToken>()))
            .Callback<VacancyTimelineEvent, CancellationToken>((vte, ct) => timelineEvent = vte);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(command.Id);

        timelineEvent.Should().NotBeNull();

        timelineEvent.VacancyId.Should().Be(command.Id);
        timelineEvent.EventType.Should().Be(TimelineEventType.Updated);
        timelineEvent.Time.Should().Be(_dateTimeMock.Object.Now);
        timelineEvent.Note.Should().Be("Job vacancy updated");
        timelineEvent.CreateBy.Should().Be(_currentUserServiceMock.Object.GetId());

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        existingJobVacancy.Title.Should().Be(command.Title);
        existingJobVacancy.ShortDescription.Should().Be(command.ShortDescription);
        existingJobVacancy.Description.Should().Be(command.Description);
        existingJobVacancy.PublishDate.Should().Be(command.PublishDate);
        existingJobVacancy.EndDate.Should().Be(command.EndDate);
        existingJobVacancy.CategoryId.Should().Be(command.CategoryId);
        existingJobVacancy.RequiredYearOfExperience.Should().Be(command.RequiredYearOfExperience);
        existingJobVacancy.WorkSchedule.Should().Be(command.WorkSchedule);
    }
}
