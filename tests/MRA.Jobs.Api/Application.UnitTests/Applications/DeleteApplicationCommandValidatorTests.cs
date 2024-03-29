﻿using MRA.Jobs.Application.Contracts.Applications.Commands.Delete;
using DeleteApplicationCommandValidator = MRA.Jobs.Application.Contracts.Applications.Commands.Delete.DeleteApplicationCommandValidator;

namespace MRA.Jobs.Application.UnitTests.Applications;
public class DeleteApplicationCommandValidatorTests : BaseTestFixture
{
    private DeleteApplicationCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteApplicationCommandValidator();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteApplicationCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteApplicationCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}
