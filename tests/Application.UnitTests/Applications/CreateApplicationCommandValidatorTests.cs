using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;
public class CreateApplicationCommandValidatorTests : BaseTestFixture
{
    private CreateApplicationCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateApplicationCommandValidator();
    }

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateApplicationCommand();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_CVEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateApplicationCommand { CV = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CV);
    }

    [Test]
    public void Validate_CoverLetterEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateApplicationCommand { CoverLetter = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CoverLetter);
    }

    [Test]
    public void Validate_ApplicantIdEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateApplicationCommand { ApplicantId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ApplicantId);
    }

    [Test]
    public void Validate_VacancyIdEmpty_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateApplicationCommand { VacancyId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VacancyId);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateApplicationCommand
        {
            ApplicantId = Guid.NewGuid(),
            VacancyId = Guid.NewGuid(),
            CV = "https://github.com/srp-internships/MRA-Jobs/pulls",
            CoverLetter = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
