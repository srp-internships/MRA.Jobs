using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Delete;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class DeleteTrainingModelCommandValidatorTests
{
    private DeleteTrainingVacancyCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteTrainingVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteTrainingVacancyCommand { Slug = string.Empty };

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
        var command = new DeleteTrainingVacancyCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}
