using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class CreateReviewerCommandValidatorTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _validator = new CreateReviewerCommandValidator();
    }

    private CreateReviewerCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateReviewerCommand request = new CreateReviewerCommand();

        // Act
        TestValidationResult<CreateReviewerCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_FieldsEmpty_ShouldFailValidation()
    {
        // Arrange 
        CreateReviewerCommand request = new CreateReviewerCommand
        {
            Avatar = "",
            FirstName = "",
            LastName = "",
            Email = "",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = ""
        };

        // Act
        TestValidationResult<CreateReviewerCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_FieldsNotEmpty_ShouldSuccessValidation()
    {
        // Arrange 
        CreateReviewerCommand request = new CreateReviewerCommand
        {
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = "+992123456789"
        };

        // Act
        TestValidationResult<CreateReviewerCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}