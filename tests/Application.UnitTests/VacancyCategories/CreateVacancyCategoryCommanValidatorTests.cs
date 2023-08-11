using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

[TestFixture]
public class CreateVacancyCategoryCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new CreateVacancyCategoryCommandValidator();
    }

    private CreateVacancyCategoryCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateVacancyCategoryCommand request = new CreateVacancyCategoryCommand();

        // Act
        TestValidationResult<CreateVacancyCategoryCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_NameEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateVacancyCategoryCommand request = new CreateVacancyCategoryCommand { Name = "" };

        // Act
        TestValidationResult<CreateVacancyCategoryCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        CreateVacancyCategoryCommand request = new CreateVacancyCategoryCommand { Name = "Test Vacancy Category" };

        // Act
        TestValidationResult<CreateVacancyCategoryCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}