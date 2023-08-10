using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class CreateReviewerCommandValidatorTests : BaseTestFixture
{
    private CreateReviewerCommandValidator _validator;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        
        _validator = new CreateReviewerCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateReviewerCommand();

        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_FieldsEmpty_ShouldFailValidation()
    {
        // Arrange 
        var request = new CreateReviewerCommand
        {
            Avatar = "",
            FirstName = "",
            LastName = "",
            Email = "",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = ""
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Test]
    public void Validate_FieldsNotEmpty_ShouldSuccessValidation()
    {
        // Arrange 
        var request = new CreateReviewerCommand
        {
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = "+992123456789"
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}