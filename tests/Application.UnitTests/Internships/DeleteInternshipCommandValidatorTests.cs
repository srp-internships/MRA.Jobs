using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Features.InternshipVacancies.Command.Delete;

namespace MRA.Jobs.Application.UnitTests.Internships;

public class DeleteInternshipCommandValidatorTests
{
    private DeleteInternshipVacancyCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteInternshipVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteInternshipVacancyCommand command = new DeleteInternshipVacancyCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<DeleteInternshipVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        DeleteInternshipVacancyCommand command = new DeleteInternshipVacancyCommand { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<DeleteInternshipVacancyCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}