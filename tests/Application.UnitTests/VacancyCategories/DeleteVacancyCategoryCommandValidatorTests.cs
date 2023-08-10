using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.DeleteVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

[TestFixture]
public class DeleteVacancyCategoryCommandValidatorTests
{
    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteVacancyCategoryCommandValidator();
    }

    private DeleteVacancyCategoryCommandValidator _validator;


    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        DeleteVacancyCategoryCommand command = new DeleteVacancyCategoryCommand { Id = Guid.Empty };

        // Act
        TestValidationResult<DeleteVacancyCategoryCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        DeleteVacancyCategoryCommand command = new DeleteVacancyCategoryCommand { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<DeleteVacancyCategoryCommand> result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}