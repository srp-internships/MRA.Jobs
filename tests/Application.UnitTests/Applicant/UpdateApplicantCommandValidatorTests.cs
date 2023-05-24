using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicants.Command.UpdateApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class UpdateApplicantCommandValidatorTests
{
    private UpdateApplicantCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateApplicantCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange 
        var command = new UpdateApplicantCommand { Id = Guid.Empty };
        
        // Act 
        var result = _validator.TestValidate(command);
        
        // Assert 
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_FirstNameIsRequired()
    {
        // Arrange
        var command = new UpdateApplicantCommand { FirstName = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Test]
    public void Validate_LastNameIsRequired()
    {
        // Arrange
        var command = new UpdateApplicantCommand { LastName = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Test]
    public void Validate_AvatarIsRequired()
    {
        // Arrange
        var command = new UpdateApplicantCommand { Avatar = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Avatar);
    }
    
    [Test]
    public void Validate_PhoneNumberIsRequired()
    {
        // Arrange
        var command = new UpdateApplicantCommand { PhoneNumber = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }
    
    [Test]
    public void Validate_PatronymicNumberIsRequired()
    {
        // Arrange
        var command = new UpdateApplicantCommand { Patronymic = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic);
    }

    [Test]
    public void Validate_EmailNumberIsRequired()
    {
        // Arrange
        var command = new UpdateApplicantCommand { Email = null };
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
}