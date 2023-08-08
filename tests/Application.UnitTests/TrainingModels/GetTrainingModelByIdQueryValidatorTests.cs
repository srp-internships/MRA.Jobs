using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Features.TrainingVacancies.Queries;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class GetTrainingModelByIdQueryValidatorTests
{
    private GetTrainingVacancyBySlugQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetTrainingVacancyBySlugQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetTrainingVacancyBySlugQuery { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetTrainingVacancyBySlugQuery { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
