using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.UpdateApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;

public class UpdateApplicationCommandValidatorTests : BaseTestFixture
{
    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateApplicationCommandValidator();
    }

    private UpdateApplicationCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        UpdateApplicationCommand request = new UpdateApplicationCommand();

        // Act
        TestValidationResult<UpdateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_CVEmpty_ShouldFailValidation()
    {
        // Arrange
        UpdateApplicationCommand request = new UpdateApplicationCommand { CV = "" };

        // Act
        TestValidationResult<UpdateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CV);
    }

    [Test]
    public void Validate_CoverLetterEmpty_ShouldFailValidation()
    {
        // Arrange
        UpdateApplicationCommand request = new UpdateApplicationCommand { CoverLetter = "" };

        // Act
        TestValidationResult<UpdateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CoverLetter);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        UpdateApplicationCommand request = new UpdateApplicationCommand
        {
            CV = "https://github.com/srp-internships/MRA-Jobs/pulls",
            CoverLetter =
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum"
        };

        // Act
        TestValidationResult<UpdateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}