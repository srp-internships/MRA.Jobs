using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

namespace MRA.Jobs.Application.UnitTests.JobVacancies;

public class GetVacancyCategoryByIdQueryValidatorTests
{
    private GetJobVacancyByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetJobVacancyByIdQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        GetJobVacancyByIdQuery query = new GetJobVacancyByIdQuery { Id = Guid.Empty };

        // Act
        TestValidationResult<GetJobVacancyByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange
        GetJobVacancyByIdQuery query = new GetJobVacancyByIdQuery { Id = Guid.NewGuid() };

        // Act
        TestValidationResult<GetJobVacancyByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}