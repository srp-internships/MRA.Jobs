using MRA.Jobs.Application.Contracts.Reviewer.Commands;
using MRA.Jobs.Application.Features.Reviewer.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
public class AddTagsToReviewerCommandValidatorTests
{
    private AddTagToReviewerCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddTagToReviewerCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ReturnsValid()
    {
        // Arrange
        var command = new AddTagsToReviewerCommand
        {
            ReviewerId = Guid.NewGuid(),
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [Test]
    public void Validate_EmptyReviewerId_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagsToReviewerCommand
        {
            ReviewerId = Guid.Empty,
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("ReviewerId", result.Errors[0].PropertyName);
    }

    [Test]
    public void Validate_EmptyTags_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagsToReviewerCommand
        {
            ReviewerId = Guid.NewGuid(),
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

