using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class GetJobVacancyByIdQueryHandlerTests : BaseTestFixture
{
    private GetJobVacancyByIdQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetJobVacancyByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    public async Task Handle_GivenValidQuery_ShouldReturnJobVacancyDetailsDTO()
    {
        var query = new GetJobVacancyByIdQuery { Id = Guid.NewGuid() };

        var jobVacancy = new JobVacancy
        {
            Id = query.Id,
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 1,
            WorkSchedule = WorkSchedule.FullTime
        };
        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>())).ReturnsAsync(jobVacancy);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(jobVacancy.Id);
        result.Title.Should().Be(jobVacancy.Title);
        result.ShortDescription.Should().Be(jobVacancy.ShortDescription);
        result.Description.Should().Be(jobVacancy.Description);
        result.PublishDate.Should().Be(jobVacancy.PublishDate);
        result.EndDate.Should().Be(jobVacancy.EndDate);
        result.CategoryId.Should().Be(jobVacancy.CategoryId);
        result.RequiredYearOfExperience.Should().Be(jobVacancy.RequiredYearOfExperience);
        result.WorkSchedule.Should().Be(jobVacancy.WorkSchedule);
    }

    [Test]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetJobVacancyByIdQuery { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((JobVacancy)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}