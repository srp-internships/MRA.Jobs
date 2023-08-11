using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Features.TrainingVacancies.Queries;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class GetTrainingModelByIdQueryValidatorTests
{
    [SetUp]
    public void Setup()
    {
        _validator = new GetTrainingVacancyByIdQueryValidator();
    }

    private GetTrainingVacancyByIdQueryValidator _validator;

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        GetTrainingVacancyByIdQuery query = new GetTrainingVacancyByIdQuery { Id = Guid.Empty };

        // Act
        TestValidationResult<GetTrainingVacancyByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        GetTrainingVacancyByIdQuery query = new GetTrainingVacancyByIdQuery { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<GetTrainingVacancyByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}