using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Create;

namespace MRA.Jobs.Application.UnitTests.Internships;

using MRA.Jobs.Application.Common.SlugGeneratorService;
using MRA.Jobs.Domain.Entities;
public class CreateInternshipCommandHandlerTests : BaseTestFixture
{
    private CreateInternshipVacancyCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateInternshipVacancyCommandHandler(
            _dbContextMock.Object,
            Mapper,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object,
            _slugGenerator.Object,
            _emailServiceMock.Object,
            _htmlServiceMock.Object);
    }

    [Test]
    [Ignore("must be Integration test ")]
    public async Task Handle_ValidRequest_ShouldCreateInternshipAndTimelineEvent()
    {
        // Arrange
        var request = new CreateInternshipVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = DateTime.UtcNow.AddDays(60),
            Duration = 10,
            Stipend = 1000
        };

        var category = new VacancyCategory { Id = request.CategoryId };
        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(category);

        var timelineEventSetMock = new Mock<DbSet<VacancyTimelineEvent>>();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timelineEventSetMock.Object);

        var InternshipSetMock = new Mock<DbSet<InternshipVacancy>>();
        var newEntityGuid = Guid.NewGuid();
        var slug = "software-developer";
        InternshipSetMock.Setup(d => d.AddAsync(It.IsAny<InternshipVacancy>(), It.IsAny<CancellationToken>())).Callback<InternshipVacancy, CancellationToken>((v, ct) => v.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.Internships).Returns(InternshipSetMock.Object);

        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(slug);

        InternshipSetMock.Verify(x => x.AddAsync(It.Is<InternshipVacancy>(i =>
            i.Title == request.Title &&
            i.ShortDescription == request.ShortDescription &&
            i.Description == request.Description &&
            i.PublishDate == request.PublishDate &&
            i.EndDate == request.EndDate &&
            i.CategoryId == request.CategoryId &&
            i.ApplicationDeadline == request.ApplicationDeadline &&
            i.Duration == request.Duration &&
            i.Stipend == request.Stipend
        ), It.IsAny<CancellationToken>()), Times.Once);

        timelineEventSetMock.Verify(x => x.AddAsync(It.Is<VacancyTimelineEvent>(te =>
            te.VacancyId == newEntityGuid &&
            te.EventType == TimelineEventType.Created &&
            te.Note == "Internship created" &&
            te.Time == _dateTimeMock.Object.Now &&
            te.CreateBy == _currentUserServiceMock.Object.GetId()
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_CategoryNotFound_ShouldThrowNotFoundException()
    {
        var request = new CreateInternshipVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = DateTime.UtcNow.AddDays(60),
            Duration = 10,
            Stipend = 1
        };

        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(() => null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
         .WithMessage($"*{nameof(VacancyCategory)}*{request.CategoryId}*");
    }
}
