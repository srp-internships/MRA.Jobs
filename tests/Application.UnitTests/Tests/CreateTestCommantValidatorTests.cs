using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Features.Tests.Commands.CreateTest;

namespace MRA.Jobs.Application.UnitTests.Tests;

[TestFixture]
public class CreateTestCommantValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new CreateTestCommandValidator();
    }

    private CreateTestCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateTestCommand request = new CreateTestCommand();

        // Act
        TestValidationResult<CreateTestCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestCommand request = new CreateTestCommand { Id = Guid.Empty };

        // Act 
        TestValidationResult<CreateTestCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_NumberOfQuestionIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestCommand request = new CreateTestCommand { NumberOfQuestion = 0 };

        // Act
        TestValidationResult<CreateTestCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfQuestion);
    }

    [Test]
    public void Validate_CategoriesIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestCommand request = new CreateTestCommand { Categories = null };

        // Act
        TestValidationResult<CreateTestCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categories);
    }

    [Test]
    public void Validate_CategoriesValueIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestCommand request = new CreateTestCommand { Categories = new List<string> { "", "" } };

        // Act
        TestValidationResult<CreateTestCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Categories);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        CreateTestCommand request = new CreateTestCommand
        {
            Id = Guid.NewGuid(), NumberOfQuestion = 10, Categories = new List<string> { "test" }
        };

        // Act
        TestValidationResult<CreateTestCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}