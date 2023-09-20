﻿using MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;

namespace MRA.Jobs.Application.UnitTests.Internships;

using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Domain.Entities;
public class GetInternshipByIdQueryHandlerTests : BaseTestFixture
{
    private GetInternshipVacancyBySlugQueryHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetInternshipVacancyBySlugQueryHandler(_dbContextMock.Object, Mapper);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnInternshipDetailsDTO()
    {
        var query = new GetInternshipVacancyBySlugQuery { Slug = "slag1" };

        var internship = new InternshipVacancy
        {
            Id = Guid.NewGuid(),
            Slug = query.Slug,
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = new DateTime(2023, 05, 20),
            Duration = 10,
            Stipend = 100
        };
        _dbContextMock.Setup(x => x.Internships.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>())).ReturnsAsync(internship);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(internship.Id);
        result.Title.Should().Be(internship.Title);
        result.ShortDescription.Should().Be(internship.ShortDescription);
        result.Description.Should().Be(internship.Description);
        result.PublishDate.Should().Be(internship.PublishDate);
        result.EndDate.Should().Be(internship.EndDate);
        result.CategoryId.Should().Be(internship.CategoryId);
        result.ApplicationDeadline.Should().Be(internship.ApplicationDeadline);
        result.Duration.Should().Be(internship.Duration);
        result.Stipend.Should().Be(internship.Stipend);
    }

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public void Handle_GivenInvalidQuery_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetInternshipVacancyBySlugQuery { Slug = "slag" };

        _dbContextMock.Setup(x => x.Internships.FindAsync(new object[] { query.Slug }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InternshipVacancy)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
