﻿using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Features.TrainingVacancies.Queries;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

public class GetTrainingModelByIdQueryHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new GetTrainingVacancyByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    private GetTrainingVacancyByIdQueryHandler _handler;

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnJobVacancyDetailsDTO()
    {
        GetTrainingVacancyByIdQuery query = new GetTrainingVacancyByIdQuery { Id = Guid.NewGuid() };

        TrainingVacancy trainingModel = new TrainingVacancy
        {
            Id = query.Id,
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            Duration = 1,
            Fees = 1
        };
        _dbContextMock
            .Setup(x => x.TrainingVacancies.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trainingModel);

        // Act
        TrainingVacancyDetailedResponse result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(trainingModel.Id);
        result.Title.Should().Be(trainingModel.Title);
        result.ShortDescription.Should().Be(trainingModel.ShortDescription);
        result.Description.Should().Be(trainingModel.Description);
        result.PublishDate.Should().Be(trainingModel.PublishDate);
        result.EndDate.Should().Be(trainingModel.EndDate);
        result.CategoryId.Should().Be(trainingModel.CategoryId);
        result.Duration.Should().Be(trainingModel.Duration);
        result.Fees.Should().Be(trainingModel.Fees);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        GetTrainingVacancyByIdQuery query = new GetTrainingVacancyByIdQuery { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x =>
                x.TrainingVacancies.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrainingVacancy)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}