using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;

namespace MRA.Jobs.Application.UnitTests.Applications;

public class GetApplicationByIdQueryValidatorTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _validator = new GetApplicationByIdQueryValidator();
    }

    private GetApplicationByIdQueryValidator _validator;

    [Test]
    public void Validate_IdIsEmpty()
    {
        // Arrange
        GetByIdApplicationQuery query = new GetByIdApplicationQuery { Id = Guid.Empty };

        // Act
        TestValidationResult<GetByIdApplicationQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty()
    {
        // Arrange
        GetByIdApplicationQuery query = new GetByIdApplicationQuery { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<GetByIdApplicationQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}