using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Features.TrainingVacancies.Commands.Delete;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class DeleteTrainingModelCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteTrainingVacancyCommandValidator();
    }

    private DeleteTrainingVacancyCommandValidator _validator;

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteTrainingVacancyCommand command = new DeleteTrainingVacancyCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<DeleteTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        DeleteTrainingVacancyCommand command = new DeleteTrainingVacancyCommand { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<DeleteTrainingVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}