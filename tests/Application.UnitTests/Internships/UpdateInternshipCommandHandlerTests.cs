using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Update;

namespace MRA.Jobs.Application.UnitTests.Internships;
public class UpdateInternshipCommandHandlerTests : BaseTestFixture
{
    private UpdateInternshipVacancyCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _handler = new UpdateInternshipVacancyCommandHandler(
            _dbContextMock.Object, Mapper, _dateTimeMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public void Handle_GivenNonExistentInternshipId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Internships.FindAsync(command.Id)).ReturnsAsync(null as InternshipVacancy);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(InternshipVacancy)}*{command.CategoryId}*");
    }

    [Test]
    public void Handle_GivenNonExistentCategoryId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand { Id = Guid.NewGuid(), CategoryId = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Internships.FindAsync(new object[] { command.Id }, CancellationToken.None)).ReturnsAsync(new InternshipVacancy());
        _dbContextMock.Setup(x => x.Categories.FindAsync(new object[] { command.CategoryId }, CancellationToken.None)).ReturnsAsync(null as VacancyCategory);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Handle_GivenValidCommand_ShouldUpdateInternshipAndAddTimelineEvent()
    {
        // Arrange
        var command = new UpdateInternshipVacancyCommand
        {
            Id = Guid.NewGuid(),
            Title = "Internship Title",
            ShortDescription = "New Short Description",
            Description = "New Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = new DateTime(2023, 05, 20),
            Duration = 10,
            Stipend = 100
        };

        var internshipsDbSetMock = new Mock<DbSet<InternshipVacancy>>();
        var categoryDbSetMock = new Mock<DbSet<VacancyCategory>>();

        _dbContextMock.Setup(c => c.Internships).Returns(internshipsDbSetMock.Object);
        _dbContextMock.Setup(c => c.Categories).Returns(categoryDbSetMock.Object);

        var existingInternship = new InternshipVacancy { Id = command.Id };
        internshipsDbSetMock.Setup(x => x.FindAsync(new object[] { command.Id }, CancellationToken.None))
            .ReturnsAsync(existingInternship);

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
        timelineEvent.Note.Should().Be("Internship updated");
        timelineEvent.CreateBy.Should().Be(_currentUserServiceMock.Object.UserId);

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        existingInternship.Title.Should().Be(command.Title);
        existingInternship.ShortDescription.Should().Be(command.ShortDescription);
        existingInternship.Description.Should().Be(command.Description);
        existingInternship.PublishDate.Should().Be(command.PublishDate);
        existingInternship.EndDate.Should().Be(command.EndDate);
        existingInternship.CategoryId.Should().Be(command.CategoryId);
        existingInternship.ApplicationDeadline.Should().Be(command.ApplicationDeadline);
        existingInternship.Duration.Should().Be(command.Duration);
        existingInternship.Stipend.Should().Be(command.Stipend);
    }
}
