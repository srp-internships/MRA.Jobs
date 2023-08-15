using MRA.Jobs.Application.Contracts.Applicant.Queries;
using MRA.Jobs.Application.Features.Applicants.Query.GetApplicantById;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class GetApplicantByIdQueryValidatorTests
{
    private GetApplicantByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetApplicantByIdQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        GetApplicantByIdQuery query = new GetApplicantByIdQuery { Id = Guid.Empty };

        // Act 
        TestValidationResult<GetApplicantByIdQuery> result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(a => a.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange 
        GetApplicantByIdQuery query = new GetApplicantByIdQuery { Id = Guid.NewGuid() };

        // Act 
        TestValidationResult<GetApplicantByIdQuery> result = _validator.TestValidate(query);

        // Assert 
        result.ShouldNotHaveValidationErrorFor(a => a.Id);
    }
}