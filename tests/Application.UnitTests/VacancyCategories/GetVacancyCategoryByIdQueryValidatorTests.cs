using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class GetVacancyCategoryByIdQueryValidatorTests
{
    private GetVacancyCategoryByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetVacancyCategoryByIdQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetVacancyCategoryByIdQuery { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetVacancyCategoryByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}