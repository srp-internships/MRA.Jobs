using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

[TestFixture]
public class DeleteJobVacancyCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteJobVacancyCommandValidator();
    }

    private DeleteJobVacancyCommandValidator _validator;

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteJobVacancyCommand command = new DeleteJobVacancyCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<DeleteJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        DeleteJobVacancyCommand command = new DeleteJobVacancyCommand { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<DeleteJobVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}