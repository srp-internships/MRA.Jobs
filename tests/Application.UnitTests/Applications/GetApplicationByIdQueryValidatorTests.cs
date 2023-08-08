using MRA.Jobs.Application.Contracts.Applications.Queries;
using MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;

namespace MRA.Jobs.Application.UnitTests.Applications;
public class GetApplicationBySlugQueryValidatorTests : BaseTestFixture
{
    private GetApplicationBySlugQueryValidator _validator;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        _validator = new GetApplicationBySlugQueryValidator();
    }

    [Test]
    public void Validate_IdIsEmpty()
    {
        // Arrange
        var query = new GetBySlugApplicationQuery { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotEmpty()
    {
        // Arrange
        var query = new GetBySlugApplicationQuery { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
