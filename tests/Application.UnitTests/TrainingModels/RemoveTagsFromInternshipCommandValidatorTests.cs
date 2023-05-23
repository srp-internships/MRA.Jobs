using MRA.Jobs.Application.Features.TrainingModels.Commands.Tags;
using MRA.Jobs.Application.Contracts.TrainingModels.Commands;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;
public class RemoveTagsFromTrainingModelsCommandValidatorTests
{
    [Test]
    public void RemoveTagFromTrainingModelsCommandValidator_Validation_Test()
    {
        var validator = new RemoveTagFromTrainingModelCommandValidator();

        var invalidCommand1 = new RemoveTagFromTrainingModelCommand
        {
            TrainingModelId = Guid.Empty,
            Tags = new string[] { "tag1", "tag2" }
        };
        var result1 = validator.Validate(invalidCommand1);
        Assert.IsFalse(result1.IsValid);

        var invalidCommand2 = new RemoveTagFromTrainingModelCommand
        {
            TrainingModelId = Guid.NewGuid(),
            Tags = new string[] { }
        };
        var result2 = validator.Validate(invalidCommand2);
        Assert.IsFalse(result2.IsValid);

        var validCommand = new RemoveTagFromTrainingModelCommand
        {
            TrainingModelId = Guid.NewGuid(),
            Tags = new string[] { "tag1", "tag2" }
        };
        var result3 = validator.Validate(validCommand);
        Assert.IsTrue(result3.IsValid);
    }

}
