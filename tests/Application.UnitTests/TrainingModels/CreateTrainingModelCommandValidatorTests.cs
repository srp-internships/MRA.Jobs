using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Create;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class CreateTrainingModelCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new CreateTrainingVacancyCommandValidator();
    }

    private CreateTrainingVacancyCommandValidator _validator;

    [Test]
    public void Validate_InvalidCommand_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand();

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveAnyValidationError();
    }

    [Test]
    public void Validate_TitleEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { Title = "" };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { ShortDescription = "" };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { Description = "" };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { CategoryId = Guid.Empty };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_DurationEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { Duration = 0 };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_FeesEmpty_ShouldFailValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand { Fees = 1 };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Fees);
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldPassValidation()
    {
        // Arrange
        CreateTrainingVacancyCommand request = new CreateTrainingVacancyCommand
        {
            Title = "Test Job Vacancy",
            ShortDescription = "Test Job Vacancy Short Description",
            Description = "Test Job Vacancy Description",
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            CategoryId = Guid.NewGuid(),
            Duration = 1,
            Fees = 1
        };

        // Act
        TestValidationResult<CreateTrainingVacancyCommand> result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}