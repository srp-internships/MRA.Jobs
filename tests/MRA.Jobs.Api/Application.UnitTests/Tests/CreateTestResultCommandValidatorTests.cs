using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Features.Tests.Commands.CreateTestResult;

namespace MRA.Jobs.Application.UnitTests.Tests;

[TestFixture]
public class CreateTestResultCommandValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _validator = new CreateTestResultCommandValidator();
    }

    private CreateTestResultCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateTestResultCommand request = new CreateTestResultCommand();

        // Act
        TestValidationResult<CreateTestResultCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TestIdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestResultCommand request = new CreateTestResultCommand { TestId = Guid.Empty };


        // Act
        TestValidationResult<CreateTestResultCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TestId);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_UserIdIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestResultCommand request = new CreateTestResultCommand { UserId = Guid.Empty };

        // Act
        TestValidationResult<CreateTestResultCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public void Validate_ScoreIsEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTestResultCommand request = new CreateTestResultCommand { Score = 0 };

        // Act
        TestValidationResult<CreateTestResultCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Score);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_AllFieldsAreValid_ShouldPassValidation()
    {
        // Arrange
        CreateTestResultCommand request = new CreateTestResultCommand
        {
            Score = 1, TestId = Guid.NewGuid(), UserId = Guid.NewGuid()
        };

        // Act 
        TestValidationResult<CreateTestResultCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}