using MRA.Jobs.Application.Contracts.TrainingModels.Queries;
using MRA.Jobs.Application.Features.TrainingModels.Queries.GetTrainingModelById;

namespace MRA.Jobs.Application.UnitTests.TrainingModels;

[TestFixture]
public class GetTrainingModelByIdQueryValidatorTests
{
    private GetTrainingModelByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetTrainingModelByIdQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetTrainingModelByIdQuery { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetTrainingModelByIdQuery { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
