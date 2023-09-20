using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class GetVacancyCategoryBySlugQueryValidatorTests
{
    private GetJobVacancyBySlugQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetJobVacancyBySlugQueryValidator();
    }

    [Test]
    [Ignore("slug")]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery { Slug ="" };


        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Test]
    [Ignore("slug")]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        var query = new GetJobVacancyBySlugQuery {Slug=""};

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Slug);
    }
}