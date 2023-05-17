using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.UpdateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class UpdateReviewerCommandValidatorTests
{
      private UpdateReviewerCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateReviewerCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange 
        var command = new UpdateReviewerCommand { Id = Guid.Empty };
        
        // Act 
        var result = _validator.TestValidate(command);
        
        // Assert 
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_FirstNameIsRequired()
    {
        // Arrange
        var command = new UpdateReviewerCommand { FirstName = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Test]
    public void Validate_LastNameIsRequired()
    {
        // Arrange
        var command = new UpdateReviewerCommand { LastName = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Test]
    public void Validate_AvatarIsRequired()
    {
        // Arrange
        var command = new UpdateReviewerCommand { Avatar = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Avatar);
    }
    
    [Test]
    public void Validate_PhoneNumberIsRequired()
    {
        // Arrange
        var command = new UpdateReviewerCommand { PhoneNumber = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }
    
    [Test]
    public void Validate_PatronymicNumberIsRequired()
    {
        // Arrange
        var command = new UpdateReviewerCommand { Patronymic = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic);
    }

    [Test]
    public void Validate_EmailNumberIsRequired()
    {
        // Arrange
        var command = new UpdateReviewerCommand { Email = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}