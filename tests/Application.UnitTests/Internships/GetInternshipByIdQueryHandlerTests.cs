﻿using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;

namespace MRA.Jobs.Application.UnitTests.Internships;

public class GetInternshipByIdQueryHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _handler = new GetInternshipVacancyByIdQueryHandler(_dbContextMock.Object, Mapper);
    }

    private GetInternshipVacancyByIdQueryHandler _handler;

    [Test]
    [Ignore("Игнорируем тест из-за TimeLine & Tag")]
    public async Task Handle_GivenValidQuery_ShouldReturnInternshipDetailsDTO()
    {
        GetInternshipVacancyByIdQuery query = new GetInternshipVacancyByIdQuery { Id = Guid.NewGuid() };

        InternshipVacancy internship = new InternshipVacancy
        {
            Id = query.Id,
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
        _dbContextMock.Setup(x => x.Internships.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(internship);

        // Act
        InternshipVacancyResponse result = await _handler.Handle(query, CancellationToken.None);

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
        GetInternshipVacancyByIdQuery query = new GetInternshipVacancyByIdQuery { Id = Guid.NewGuid() };

        _dbContextMock.Setup(x => x.Internships.FindAsync(new object[] { query.Id }, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InternshipVacancy)null);

        // Act + Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}