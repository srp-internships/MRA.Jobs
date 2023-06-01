using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.CreateJobVacancyTest;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

[TestFixture]
public class CreateJobVacancyTestCommantValidatorTests
{
    private CreateJobVacancyTestCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateJobVacancyTestCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyTestCommand();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyTestCommand() { Id = Guid.Empty };

        // Act 
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_NumberOfQuestionIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyTestCommand() { NumberOfQuestion = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfQuestion);
    }

    [Test]
    public void Validate_CategoriesIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyTestCommand() { Categories = null };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categories);
    }

    [Test]
    public void Validate_CategoriesValueIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateJobVacancyTestCommand()
        {
            Categories = new List<string>() { "", "" }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categories);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateJobVacancyTestCommand()
        {
            Id = Guid.NewGuid(),
            NumberOfQuestion = 10,
            Categories = new List<string>()
            {
                "test"
            }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}