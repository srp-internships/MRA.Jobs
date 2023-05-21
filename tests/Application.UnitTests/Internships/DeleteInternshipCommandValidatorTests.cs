using MRA.Jobs.Application.Contracts.Internships.Commands;
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
        var command = new DeleteInternshipVacancyCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteInternshipVacancyCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
