using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Features.Tests.Commands.CreateTest;

namespace MRA.Jobs.Application.UnitTests.Tests;

[TestFixture]
public class CreateTestCommantValidatorTests
{
    private CreateTestCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateTestCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestCommand();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    [Ignore("slug")]
    public void Validate_IdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestCommand() { Id = Guid.Empty };

        // Act 
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_NumberOfQuestionIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestCommand() { NumberOfQuestion = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfQuestion);
    }

    [Test]
    public void Validate_CategoriesIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestCommand() { Categories = null };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categories);
    }

    [Test]
    public void Validate_CategoriesValueIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestCommand()
        {
            Categories = new List<string>() { "", "" }
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categories);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateTestCommand()
        {
            Slug=string.Empty,
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