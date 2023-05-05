using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class CreateJobVacancyCommandHandlerTests : BaseTestFixture
{
    private CreateJobVacancyCommandHandler _handler;

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

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateJobVacancyAndTimelineEvent()
    {
        // Arrange
        var request = new CreateJobVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = 1,
            RequiredYearOfExperience = 2,
            WorkSchedule = WorkSchedule.FullTime
        };

        var category = new VacancyCategory { Id = 1 };
        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(category);

        var timelineEventSetMock = new Mock<DbSet<VacancyTimelineEvent>>();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timelineEventSetMock.Object);

        var jobVacancySetMock = new Mock<DbSet<JobVacancy>>();
        jobVacancySetMock.Setup(d => d.AddAsync(It.IsAny<JobVacancy>(), It.IsAny<CancellationToken>())).Callback<JobVacancy, CancellationToken>((v, ct) => v.Id = 2);
        _dbContextMock.Setup(x => x.JobVacancies).Returns(jobVacancySetMock.Object);

        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(2);

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
            te.VacancyId == category.Id + 1 &&
            te.EventType == TimelineEventType.Created &&
            te.Note == "Job vacancy created" &&
            te.Time == _dateTimeMock.Object.Now &&
            te.CreateBy == _currentUserServiceMock.Object.UserId
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_CategoryNotFound_ShouldThrowNotFoundException()
    {
        var request = new CreateJobVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = 1,
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
