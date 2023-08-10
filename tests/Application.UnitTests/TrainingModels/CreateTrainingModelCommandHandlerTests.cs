﻿using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Create;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

public class CreateTrainingModelCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateTrainingVacancyCommandHandler(
            _dbContextMock.Object,
            Mapper,
            _dateTimeMock.Object,
            _currentUserServiceMock.Object);
    }

    private CreateTrainingVacancyCommandHandler _handler;

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateTrainingModelAndTimelineEvent()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = Guid.NewGuid(),
            Duration = 2,
            Fees = 2
        };

        VacancyCategory category = new VacancyCategory { Id = request.CategoryId };
        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(category);

        Mock<DbSet<VacancyTimelineEvent>> timelineEventSetMock = new Mock<DbSet<VacancyTimelineEvent>>();
        _dbContextMock.Setup(x => x.VacancyTimelineEvents).Returns(timelineEventSetMock.Object);

        Mock<DbSet<TrainingVacancy>> trainingModelSetMock = new Mock<DbSet<TrainingVacancy>>();
        Guid newEntityGuid = Guid.NewGuid();
        trainingModelSetMock.Setup(d => d.AddAsync(It.IsAny<TrainingVacancy>(), It.IsAny<CancellationToken>()))
            .Callback<TrainingVacancy, CancellationToken>((v, ct) => v.Id = newEntityGuid);
        _dbContextMock.Setup(x => x.TrainingVacancies).Returns(trainingModelSetMock.Object);

        _dateTimeMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
        _currentUserServiceMock.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        Guid result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().Be(newEntityGuid);

        trainingModelSetMock.Verify(x => x.AddAsync(It.Is<TrainingVacancy>(jv =>
            jv.Title == request.Title &&
            jv.ShortDescription == request.ShortDescription &&
            jv.Description == request.Description &&
            jv.PublishDate == request.PublishDate &&
            jv.EndDate == request.EndDate &&
            jv.CategoryId == request.CategoryId &&
            jv.Duration == request.Duration &&
            jv.Fees == request.Fees
        ), It.IsAny<CancellationToken>()), Times.Once);

        timelineEventSetMock.Verify(x => x.AddAsync(It.Is<VacancyTimelineEvent>(te =>
            te.VacancyId == newEntityGuid &&
            te.EventType == TimelineEventType.Created &&
            te.Note == "Training Model created" &&
            te.Time == _dateTimeMock.Object.Now &&
            te.CreateBy == _currentUserServiceMock.Object.GetId()
        ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_CategoryNotFound_ShouldThrowNotFoundException()
    {
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand
        {
            Title = "Software Developer",
            ShortDescription = "Join our team of talented developers",
            Description = "We're looking for a software developer to help us build amazing products",
            PublishDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(30),
            CategoryId = Guid.NewGuid(),
            Duration = 2,
            Fees = 2
        };

        _dbContextMock.Setup(x => x.Categories.FindAsync(request.CategoryId)).ReturnsAsync(() => null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(VacancyCategory)}*{request.CategoryId}*");
    }
}