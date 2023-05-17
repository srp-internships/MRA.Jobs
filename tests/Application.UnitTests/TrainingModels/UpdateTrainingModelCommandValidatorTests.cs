using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Features.TraningModels.Commands.UpdateTraningModel;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class UpdateTrainingModelCommandValidatorTests
{
    private UpdateTrainingModelCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateTrainingModelCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_TitleIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { Title = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validate_ShortDescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { ShortDescription = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
    }

    [Test]
    public void Validate_DescriptionIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { Description = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validate_PublishDateIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { PublishDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PublishDate);
    }

    [Test]
    public void Validate_EndDateIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { EndDate = DateTime.MinValue };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Test]
    public void Validate_CategoryIdIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { CategoryId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Test]
    public void Validate_DurationIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { Duration = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Duration);
    }

    [Test]
    public void Validate_FeesIsRequired()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand { Fees = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Fees);
    }

    [Test]
    public void Validate_GivenValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new UpdateTrainingModelCommand
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
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
