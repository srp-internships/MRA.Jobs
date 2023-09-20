using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

[TestFixture]
public class DeleteVacancyCategoryCommandValidatorTests
{
    private DeleteVacancyCategoryCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteVacancyCategoryCommandValidator();
    }


    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteVacancyCategoryCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteVacancyCategoryCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }

}
