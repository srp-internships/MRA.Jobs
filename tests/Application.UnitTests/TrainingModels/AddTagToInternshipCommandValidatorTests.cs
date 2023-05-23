using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Features.TraningModels.Commands.Tags;

namespace MRA.Jobs.Application.UnitTests.TrainingModel;
public class AddTagToTrainingModelCommandValidatorTests
{
    private AddTagToTrainingModelCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddTagToTrainingModelCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ReturnsValid()
    {
        // Arrange
        var command = new AddTagToTrainingModelCommand
        {
            TrainingModelId = Guid.NewGuid(),
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [Test]
    public void Validate_EmptyTrainingModelId_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagToTrainingModelCommand
        {
            TrainingModelId = Guid.Empty,
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("TrainingModelId", result.Errors[0].PropertyName);
    }

    [Test]
    public void Validate_EmptyTags_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagToTrainingModelCommand
        {
            TrainingModelId = Guid.NewGuid(),
            Tags = new string[] { }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("Tags", result.Errors[0].PropertyName);
    }
}

