using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;
[TestFixture]
public class CreateVacancyCategoryCommandValidatorTests
{
    private CreateVacancyCategoryCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateVacancyCategoryCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateVacancyCategoryCommand();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_NameEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateVacancyCategoryCommand { Name = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateVacancyCategoryCommand
        {
            Name = "Test Vacancy Category"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}