using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.CreateApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class CreateApplicantCommandValidatorTests : BaseTestFixture
{
    private CreateApplicantCommandValidator _validator;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        
        _validator = new CreateApplicantCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateApplicantCommand();

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
        var request = new CreateApplicantCommand
        {
            Avatar = "",
            FirstName = "",
            LastName = "",
            Email = "",
            Patronymic = "",
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
        var request = new CreateApplicantCommand
        {
            Avatar = "user_avatar",
            FirstName = "userFirstname",
            LastName = "userLastname",
            Email = "user@gmail.com",
            Patronymic = "userPatronymic",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = "+992123456789"
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}