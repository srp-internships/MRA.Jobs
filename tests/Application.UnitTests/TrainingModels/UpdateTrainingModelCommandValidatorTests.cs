using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Update;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class UpdateTrainingModelCommandValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _validator = new UpdateTrainingVacancyCommandValidator();
    }

    private UpdateTrainingVacancyCommandValidator _validator;

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_TitleIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { Title = null };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { ShortDescription = null };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { Description = null };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { CategoryId = Guid.Empty };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_DurationIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { Duration = 0 };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_FeesIsRequired()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand { Fees = 0 };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Fees);
    }

    [Test]
    public void Validate_GivenValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        UpdateTrainingVacancyCommand command = new UpdateTrainingVacancyCommand
        {
            Id = Guid.NewGuid(),
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.NewGuid(),
            Duration = 10,
            Fees = 1000
        };

        // Act
        TestValidationResult<UpdateTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}