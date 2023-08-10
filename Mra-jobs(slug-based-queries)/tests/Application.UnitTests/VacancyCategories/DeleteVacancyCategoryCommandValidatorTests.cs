using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Features.VacancyCategories.Command.DeleteVacancyCategory;

namespace MRA.Jobs.Application.UnitTests.VacancyCategories;

[TestFixture]
public class DeleteVacancyCategoryCommandValidatorTests
{
    private DeleteVacancyCategoryCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteVacancyCategoryCommandValidator();
    }


    [Test]
    public void Validate_IdIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        var command = new DeleteVacancyCategoryCommand { Id = Guid.Empty };

        // Act
       var result =_validator.TestValidate(command);

        // Assert
       result.ShouldHaveValidationErrorFor(x=>x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty_ShouldNotReturnValidationError()
    {
        // Arrange
        var command = new DeleteVacancyCategoryCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
    
}
