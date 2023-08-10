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
        GetVacancyCategoryByIdQuery query = new GetVacancyCategoryByIdQuery { Id = Guid.Empty };

        // Act
        TestValidationResult<GetVacancyCategoryByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        GetVacancyCategoryByIdQuery query = new GetVacancyCategoryByIdQuery { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<GetVacancyCategoryByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}