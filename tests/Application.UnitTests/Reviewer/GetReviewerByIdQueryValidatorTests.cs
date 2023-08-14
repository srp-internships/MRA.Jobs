using MRA.Jobs.Application.Contracts.Reviewer.Queries;
using MRA.Jobs.Application.Features.Reviewer.Query.GetReviewerById;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class GetReviewerByIdQueryValidatorTests
{
    private GetReviewerByIdQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetReviewerByIdQueryValidator();
    }

    [Test]
    public void Validate_IdIsZero()
    {
        // Arrange
        var query = new GetReviewerByIdQuery { Id = Guid.Empty };
        
        // Act 
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(a => a.Id);
    }

    [Test]
    public void Validate_IdIsNotZero()
    {
        // Arrange 
        var query = new GetReviewerByIdQuery { Id = Guid.NewGuid() };
        
        // Act 
        var result = _validator.TestValidate(query);
        
        // Assert 
        result.ShouldNotHaveValidationErrorFor(a => a.Id);
    }
}