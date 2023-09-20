using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;

namespace MRA.Jobs.Application.UnitTests.Internships;

[TestFixture]
public class GetInternshipByIdQueryValidatorTests
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
        var command = new DeleteInternshipVacancyCommand { Slug = string.Empty };

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
        var command = new DeleteInternshipVacancyCommand { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}
