﻿using MRA.Jobs.Application.Contracts.Internships.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.Internships.Command.Tags;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;

namespace MRA.Jobs.Application.UnitTests.Intirnship;
public class RemoveTagsFromInternshipCommandValidatorTests
{
    [Test]
    public void RemoveTagFromInternshipCommandValidator_Validation_Test()
    {
        var validator = new RemoveTagFromInternshipCommandValidator();

        var invalidCommand1 = new RemoveTagFromInternshipCommand
        {
            InternshipId = Guid.Empty,
            Tags = new string[] { "tag1", "tag2" }
        };
        var result1 = validator.Validate(invalidCommand1);
        Assert.IsFalse(result1.IsValid);

        var invalidCommand2 = new RemoveTagFromInternshipCommand
        {
            InternshipId = Guid.NewGuid(),
            Tags = new string[] { }
        };
        var result2 = validator.Validate(invalidCommand2);
        Assert.IsFalse(result2.IsValid);

        var validCommand = new RemoveTagFromInternshipCommand
        {
            InternshipId = Guid.NewGuid(),
            Tags = new string[] { "tag1", "tag2" }
        };
        var result3 = validator.Validate(validCommand);
        Assert.IsTrue(result3.IsValid);
    }

}
