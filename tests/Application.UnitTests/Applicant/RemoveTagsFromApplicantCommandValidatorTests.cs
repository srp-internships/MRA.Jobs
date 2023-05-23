using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.Tags;

namespace MRA.Jobs.Application.UnitTests.Applicant;
public class RemoveTagsFromApplicantCommandValidatorTests
{
    [Test]
    public void RemoveTagFromApplicantCommandValidator_Validation_Test()
    {
        var validator = new RemoveTagFromApplicantCommandValidator();

        var invalidCommand1 = new RemoveTagsFromApplicantCommand
        {
            ApplicantId = Guid.Empty,
            Tags = new string[] { "tag1", "tag2" }
        };
        var result1 = validator.Validate(invalidCommand1);
        Assert.IsFalse(result1.IsValid);

        var invalidCommand2 = new RemoveTagsFromApplicantCommand
        {
            ApplicantId = Guid.NewGuid(),
            Tags = new string[] { }
        };
        var result2 = validator.Validate(invalidCommand2);
        Assert.IsFalse(result2.IsValid);

        var validCommand = new RemoveTagsFromApplicantCommand
        {
            ApplicantId = Guid.NewGuid(),
            Tags = new string[] { "tag1", "tag2" }
        };
        var result3 = validator.Validate(validCommand);
        Assert.IsTrue(result3.IsValid);
    }

}
