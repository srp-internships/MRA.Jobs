namespace MRA.Jobs.Application.UnitTests.TrainingModels;

using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Features.TrainingVacancies.Queries;
using MRA.Jobs.Domain.Entities;
public class GetTrainingModelByIdQueryHandlerTests : BaseTestFixture
{
    private GetTrainingVacancyBySlugQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new GetTrainingVacancyBySlugQueryHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnJobVacancyDetailsDTO()
    {
        var query = new GetTrainingVacancyBySlugQuery { Slug="" };

        var trainingModel = new TrainingVacancy
        {
            Id = Guid.NewGuid(),
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            Duration = 1,
            Fees = 1
        };
        _dbContextMock.Setup(x => x.TrainingVacancies.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>())).ReturnsAsync(trainingModel);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

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
        var query = new GetTrainingVacancyBySlugQuery { Slug = "" };

        _dbContextMock.Setup(x => x.TrainingVacancies.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrainingVacancy)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
