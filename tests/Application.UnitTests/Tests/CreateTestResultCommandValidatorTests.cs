using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Features.Tests.Commands.CreateTestResult;

namespace MRA.Jobs.Application.UnitTests.Tests;

[TestFixture]
public class CreateTestResultCommandValidatorTests
{
    private CreateTestResultCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new CreateTestResultCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestResultCommand();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TestIdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestResultCommand { TestId = Guid.Empty };


        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TestId);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_UserIdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestResultCommand { UserId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public void Validate_ScoreIsEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateTestResultCommand { Score = 0 };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Score);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_AllFieldsAreValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateTestResultCommand
        {
            Score = 1,
            TestId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        // Act 
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}