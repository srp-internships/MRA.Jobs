
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;


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
    [Ignore("Slug")]
    public void Validate_IdIsEmpty()
    {
        // Arrange
        var query = new GetBySlugApplicationQuery { Slug = string.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    public void Validate_SlugIsNotEmpty()
    {
        // Arrange
        var query = new GetBySlugApplicationQuery { Slug = "gdrgdrgrdgdrgdr" };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}
