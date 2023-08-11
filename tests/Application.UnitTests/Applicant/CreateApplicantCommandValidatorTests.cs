using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicants.Command.CreateApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class CreateApplicantCommandValidatorTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _validator = new CreateApplicantCommandValidator();
    }

    private CreateApplicantCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateApplicantCommand request = new CreateApplicantCommand();

        // Act
        TestValidationResult<CreateApplicantCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_FieldsEmpty_ShouldFailValidation()
    {
        // Arrange 
        CreateApplicantCommand request = new CreateApplicantCommand
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
        TestValidationResult<CreateApplicantCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_FieldsNotEmpty_ShouldSuccessValidation()
    {
        // Arrange 
        CreateApplicantCommand request = new CreateApplicantCommand
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
        TestValidationResult<CreateApplicantCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}