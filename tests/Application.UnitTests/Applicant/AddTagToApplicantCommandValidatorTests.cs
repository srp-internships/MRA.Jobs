using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Applicant;
public class AddTagToApplicantCommandValidatorTests
{
    private AddTagToApplicantCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddTagToApplicantCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ReturnsValid()
    {
        // Arrange
        var command = new AddTagsToApplicantCommand
        {
            ApplicantId = Guid.NewGuid(),
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [Test]
    public void Validate_EmptyApplicantId_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagsToApplicantCommand
        {
            ApplicantId = Guid.Empty,
            Tags = new string[] { "Tag1", "Tag2" }
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count);
        Assert.AreEqual("ApplicantId", result.Errors[0].PropertyName);
    }

    [Test]
    public void Validate_EmptyTags_ReturnsInvalid()
    {
        // Arrange
        var command = new AddTagsToApplicantCommand
        {
            ApplicantId = Guid.NewGuid(),
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

