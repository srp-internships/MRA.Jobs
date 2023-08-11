using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Application.Features.Applications.Command.DeleteApplication;

namespace MRA.Jobs.Application.UnitTests.Applications;

public class DeleteApplicationCommandValidatorTests : BaseTestFixture
{
    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteApplicationCommandValidator();
    }

    private DeleteApplicationCommandValidator _validator;

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteApplicationCommand command = new DeleteApplicationCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<DeleteApplicationCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        DeleteApplicationCommand command = new DeleteApplicationCommand { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<DeleteApplicationCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}