using MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;
using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.UnitTests.Applications;
using MRA.Jobs.Domain.Entities;
public class UpdateApplicationCommandHandlerTests : BaseTestFixture
{
    private UpdateApplicationCommandHadler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new UpdateApplicationCommandHadler(
            _dbContextMock.Object, Mapper, _dateTimeMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public void Handle_GivenNonExistentApplicationId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateApplicationCommand { Id = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applications.FindAsync(command.Id)).ReturnsAsync(null as Application);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);


        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Application)}*{command.Id}*");
    }

    [Test]
    public void Handle_GivenNonExistentVacancyId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateApplicationCommand { Id = Guid.NewGuid(), VacancyId = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applications.FindAsync(new object[] { command.Id }, CancellationToken.None)).ReturnsAsync(new Application());
        _dbContextMock.Setup(x => x.Vacancies.FindAsync(new object[] { command.VacancyId }, CancellationToken.None)).ReturnsAsync(null as Vacancy);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Handle_GivenNonExistentApplicantId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateApplicationCommand { Id = Guid.NewGuid(), ApplicantId = Guid.NewGuid() };
        _dbContextMock.Setup(x => x.Applications.FindAsync(new object[] { command.Id }, CancellationToken.None)).ReturnsAsync(new Application());
        _dbContextMock.Setup(x => x.Applicants.FindAsync(new object[] { command.ApplicantId }, CancellationToken.None)).ReturnsAsync(null as Applicant);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Handle_GivenValidCommand_ShouldUpdateApplicationAndAddTimelineEvent()
    {
        // Arrange
        var command = new UpdateApplicationCommand
        {
            Id = Guid.NewGuid(),
            VacancyId = Guid.NewGuid(),
            ApplicantId = Guid.NewGuid(),
            CoverLetter = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.\r\n\r\n",
            CV = "https://www.lipsum.com/",
            StatusId = (int)ApplicationStatus.Submitted
        };

        var existingApplication = new Application { Id = command.Id };
        _dbContextMock.Setup(x => x.Applications.FindAsync(command.Id))
            .ReturnsAsync(existingApplication);

        var vacancy = new JobVacancy { Id = command.VacancyId };
        _dbContextMock.Setup(x => x.Vacancies.FindAsync(command.VacancyId))
            .ReturnsAsync(vacancy);

        var applicant = new Applicant { Id = command.ApplicantId };
        _dbContextMock.Setup(x => x.Applicants.FindAsync(command.ApplicantId))
            .ReturnsAsync(applicant);

        var timelineEvent = new ApplicationTimelineEvent();
        _dbContextMock.Setup(x => x.ApplicationTimelineEvents.AddAsync(It.IsAny<ApplicationTimelineEvent>(), It.IsAny<CancellationToken>()))
            .Callback<ApplicationTimelineEvent, CancellationToken>((vte, ct) => timelineEvent = vte);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(command.Id);

        timelineEvent.Should().NotBeNull();

        timelineEvent.ApplicationId.Should().Be(command.Id);
        timelineEvent.EventType.Should().Be(TimelineEventType.Updated);
        timelineEvent.Time.Should().Be(_dateTimeMock.Object.Now);
        timelineEvent.Note.Should().Be("Application updated");
        timelineEvent.CreateBy.Should().Be(_currentUserServiceMock.Object.GetId());

        _dbContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);

        existingApplication.ApplicantId.Should().Be(command.ApplicantId);
        existingApplication.VacancyId.Should().Be(command.VacancyId);
        existingApplication.CoverLetter.Should().Be(existingApplication.CoverLetter);
        existingApplication.CV.Should().Be(command.CV);
        ((int)existingApplication.Status).Should().Be(command.StatusId);
    }
}
