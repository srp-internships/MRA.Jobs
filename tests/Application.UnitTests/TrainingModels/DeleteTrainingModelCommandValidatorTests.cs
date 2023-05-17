using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Features.TraningModels.Commands.DeleteTraningModel;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class DeleteTrainingModelCommandValidatorTests
{
    private DeleteTrainingModelCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteTrainingModelCommandValidator();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteTrainingModelCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteTrainingModelCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
