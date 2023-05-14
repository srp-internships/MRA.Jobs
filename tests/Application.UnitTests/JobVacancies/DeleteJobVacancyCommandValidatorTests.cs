using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

[TestFixture]
public class DeleteJobVacancyCommandValidatorTests
{
    private DeleteJobVacancyCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteJobVacancyCommandValidator();
    }

    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteJobVacancyCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteJobVacancyCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}