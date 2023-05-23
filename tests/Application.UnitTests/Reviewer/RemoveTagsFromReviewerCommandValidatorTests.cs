using FluentValidation;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;
using MRA.Jobs.Application.Features.Reviewer.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
public class RemoveTagFromReviewerCommandValidatorTests
{
    [Test]
    public void RemoveTagFromReviewerCommandValidator_Validation_Test()
    {
        var validator = new RemoveTagFromReviewerCommandValidator();

        var invalidCommand1 = new RemoveTagsFromReviewerCommand
        {
            ReviewerId = Guid.Empty,
            Tags = new string[] { "tag1", "tag2" }
        };
        var result1 = validator.Validate(invalidCommand1);
        Assert.IsFalse(result1.IsValid);

        var invalidCommand2 = new RemoveTagsFromReviewerCommand
        {
            ReviewerId = Guid.NewGuid(),
            Tags = new string[] { }
        };
        var result2 = validator.Validate(invalidCommand2);
        Assert.IsFalse(result2.IsValid);

        var validCommand = new RemoveTagsFromReviewerCommand
        {
            ReviewerId = Guid.NewGuid(),
            Tags = new string[] { "tag1", "tag2" }
        };
        var result3 = validator.Validate(validCommand);
        Assert.IsTrue(result3.IsValid);
    }

}
