﻿using MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyBySlug;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Domain.Entities;
public class GetVacancyCommandByIdQueryHandlerTests : BaseTestFixture
{
    private GetJobVacancyBySlugQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetJobVacancyBySlugQueryHandler(_dbContextMock.Object, Mapper, _currentUserServiceMock.Object);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnJobVacancyDetailsDTO()
    {
        var query = new GetJobVacancyBySlugQuery { Slug = "jobs-jobtitle" };

        var jobVacancy = new JobVacancy
        {
            Id = Guid.NewGuid(),
            Slug = "jobs-jobtitle",
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            RequiredYearOfExperience = 1,
            WorkSchedule = WorkSchedule.FullTime
        };
        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>())).ReturnsAsync(jobVacancy);

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
        result.WorkSchedule.Should().Be(Mapper.Map<Contracts.Dtos.Enums.ApplicationStatusDto.WorkSchedule>(jobVacancy.WorkSchedule));
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery { Slug = "qwe" };

        _dbContextMock.Setup(x => x.JobVacancies.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((JobVacancy)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}