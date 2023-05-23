using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Features.Internships.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Internship;
public class AddTagToInternshipCommandValidatorTests
{
    private AddTagToInternshipCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddTagToInternshipCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ReturnsValid()
    {
        // Arrange
        var command = new AddTagToInternshipCommand
        {
            InternshipId = Guid.NewGuid(),
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [Test]
    public void Validate_EmptyInternshipId_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagToInternshipCommand
        {
            InternshipId = Guid.Empty,
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("InternshipId", result.Errors[0].PropertyName);
    }

    [Test]
    public void Validate_EmptyTags_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagToInternshipCommand
        {
            InternshipId = Guid.NewGuid(),
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

