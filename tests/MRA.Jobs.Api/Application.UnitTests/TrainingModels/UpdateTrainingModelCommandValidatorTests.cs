using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class UpdateTrainingModelCommandValidatorTests
{
    private UpdateTrainingVacancyCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateTrainingVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    public void Validate_TitleIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Title = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { ShortDescription = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Description = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    [Ignore("update")]
    public void Validate_PublishDateIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { PublishDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    [Ignore("update")]
    public void Validate_EndDateIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { EndDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { CategoryId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_DurationIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Duration = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_FeesIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand { Fees = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Fees);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_GivenValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new UpdateTrainingVacancyCommand
        {
            Slug=string.Empty,
            Title = "Job Title",
            ShortDescription = "Short Description",
            Description = "Job Description",
            PublishDate = new DateTime(2023, 05, 05),
            EndDate = new DateTime(2023, 05, 06),
            CategoryId = Guid.Empty,
            Duration = 10,
            Fees = 1000
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
