using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class GetVacancyCategoryBySlugQueryValidatorTests
{
    private GetVacancyCategoryBySlugQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetVacancyCategoryBySlugQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetVacancyCategoryBySlugQuery { Slug="" };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetVacancyCategoryBySlugQuery { Slug = "" };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}