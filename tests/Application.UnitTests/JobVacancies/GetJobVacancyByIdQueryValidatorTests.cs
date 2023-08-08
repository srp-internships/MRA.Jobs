using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class GetVacancyCategoryBySlugQueryValidatorTests
{
    private GetJobVacancyBySlugQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetJobVacancyBySlugQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}