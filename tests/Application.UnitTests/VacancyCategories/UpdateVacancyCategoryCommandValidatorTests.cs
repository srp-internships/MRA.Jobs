using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

public class UpdateVacancyCategoryCommandValidatorTests
{
    private UpdateVacancyCategoryCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateVacancyCategoryCommandValidator();
    }

    [Test]
    public void Validate_IdIsRequired()
    {
        // Arrange
        var command = new UpdateVacancyCategoryCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_NameIsRequired()
    {
        // Arrange
        var command = new UpdateVacancyCategoryCommand { Name = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}
