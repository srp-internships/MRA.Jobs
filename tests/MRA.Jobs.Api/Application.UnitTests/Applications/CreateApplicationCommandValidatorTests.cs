using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.CreateApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;

public class CreateApplicationCommandValidatorTests : BaseTestFixture
{
    [SetUp]
    public void SetUp()
    {
        _validator = new CreateApplicationCommandValidator();
    }

    private CreateApplicationCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateApplicationCommand request = new CreateApplicationCommand();

        // Act
        TestValidationResult<CreateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_CoverLetterEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateApplicationCommand request = new CreateApplicationCommand { CoverLetter = "" };

        // Act
        TestValidationResult<CreateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CoverLetter);
    }

    //TODO
    //[Test]
    //public void Validate_ApplicantIdEmpty_ShouldFailValidation()
    //{
    //    // Arrange
    //    CreateApplicationCommand request = new CreateApplicationCommand ();

    //    // Act
    //    TestValidationResult<CreateApplicationCommand> result = _validator.TestValidate(request);

    //    // Assert
    //    result.ShouldHaveValidationErrorFor(x => x.ApplicantId);
    //}

    [Test]
    public void Validate_VacancyIdEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateApplicationCommand request = new CreateApplicationCommand { VacancyId = Guid.Empty };

        // Act
        TestValidationResult<CreateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.VacancyId);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        CreateApplicationCommand request = new CreateApplicationCommand
        {
            VacancyId = Guid.NewGuid(),
            CoverLetter =
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum"
        };

        // Act
        TestValidationResult<CreateApplicationCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}