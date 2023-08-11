using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class CreateJobVacancyCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateJobVacancyCommandHandler(
            _dbContextMock.Object,
            Mapper,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object);
    }

    private CreateJobVacancyCommandHandler _handler;

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateJobVacancyAndTimelineEvent()
    {
        // Arrange
        CreateJobVacancyCommand request = new CreateJobVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 2,
            WorkSchedule = WorkSchedule.FullTime
        };

        VacancyCategory category = new VacancyCategory { Id = request.CategoryId };
        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(category);

        Mock<DbSet<VacancyTimelineEvent>> timelineEventSetMock = new Mock<DbSet<VacancyTimelineEvent>>();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timelineEventSetMock.Object);

        Mock<DbSet<JobVacancy>> jobVacancySetMock = new Mock<DbSet<JobVacancy>>();
        Guid newEntityGuid = Guid.NewGuid();
        jobVacancySetMock.Setup(d => d.AddAsync(It.IsAny<JobVacancy>(), It.IsAny<CancellationToken>()))
            .Callback<JobVacancy, CancellationToken>((v, ct) => v.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.JobVacancies).Returns(jobVacancySetMock.Object);

        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        Guid result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(newEntityGuid);

        jobVacancySetMock.Verify(x => x.AddAsync(It.Is<JobVacancy>(jv =>
            jv.Title == request.Title &&
            jv.ShortDescription == request.ShortDescription &&
            jv.Description == request.Description &&
            jv.PublishDate == request.PublishDate &&
            jv.EndDate == request.EndDate &&
            jv.CategoryId == request.CategoryId &&
            jv.RequiredYearOfExperience == request.RequiredYearOfExperience &&
            jv.WorkSchedule == request.WorkSchedule
        ), It.IsAny<CancellationToken>()), Times.Once);

        timelineEventSetMock.Verify(x => x.AddAsync(It.Is<VacancyTimelineEvent>(te =>
            te.VacancyId == newEntityGuid &&
            te.EventType == TimelineEventType.Created &&
            te.Note == "Job vacancy created" &&
            te.Time == _dateTimeMock.Object.Now &&
            te.CreateBy == _currentUserServiceMock.Object.GetId()
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_CategoryNotFound_ShouldThrowNotFoundException()
    {
        CreateJobVacancyCommand request = new CreateJobVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 2,
            WorkSchedule = WorkSchedule.FullTime
        };

        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(() => null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(VacancyCategory)}*{request.CategoryId}*");
    }
}