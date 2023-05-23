using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

namespace MRA.Jobs.Application.UnitTests.JobVacancy;
public class AddTagToJobVacancyCommandValidatorTests
{
    private AddTagToJobVacancyCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddTagToJobVacancyCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ReturnsValid()
    {
        // Arrange
        var command = new AddTagsToJobVacancyCommand
        {
            JobVacancyId = Guid.NewGuid(),
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [Test]
    public void Validate_EmptyJobVacancyId_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagsToJobVacancyCommand
        {
            JobVacancyId = Guid.Empty,
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("JobVacancyId", result.Errors[0].PropertyName);
    }

    [Test]
    public void Validate_EmptyTags_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagsToJobVacancyCommand
        {
            JobVacancyId = Guid.NewGuid(),
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

