using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Create;

namespace MRA.Jobs.Application.UnitTests.Internships;

[TestFixture]
public class CreateInternshipCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new CreateInternshipVacancyCommandValidator();
    }

    private CreateInternshipVacancyCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        //Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand();

        //Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TitleEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { Title = "" };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { ShortDescription = "" };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { Description = "" };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { CategoryId = Guid.Empty };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_ApplicationDeadlineEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request =
            new CreateInternshipVacancyCommand { ApplicationDeadline = DateTime.MinValue };

        // Act 
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ApplicationDeadline);
    }

    [Test]
    public void Validate_DurationEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { Duration = 0 };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_StipendEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand { Stipend = 0 };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Stipend);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        CreateInternshipVacancyCommand request = new CreateInternshipVacancyCommand
        {
            Title = "Test Internship",
            ShortDescription = "Test Internship Short Description",
            Description = "Test Internship Description",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            CategoryId = Guid.NewGuid(),
            ApplicationDeadline = DateTime.Now.AddDays(60),
            Duration = 10,
            Stipend = 1
        };

        // Act
        TestValidationResult<CreateInternshipVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}